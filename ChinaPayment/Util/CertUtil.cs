using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using log4net;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Pkix;
using Org.BouncyCastle.Utilities.Collections;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;

namespace ChinaPayment.Util
{
    public class Cert
    {
        public AsymmetricKeyParameter AsymmetricKey { get; set; }
        public X509Certificate X509Certificate { get; set; }
        public string CertId { get; set; }
    }

    /// <summary>
    /// 证书操作工具类
    /// </summary>
    public class CertUtil
    {
        /// <summary>
        /// 日志
        /// </summary>
        private static readonly ILog Log = LogHelper.GetLogger(typeof(CertUtil));

        //签名证书，key是路径
        private static readonly Dictionary<string, Cert> SignCerts = new Dictionary<string, Cert>();
        //5.0.0验签证书，key是certId
        private static readonly Dictionary<string, Cert> CerCerts = new Dictionary<string, Cert>();
        //加密证书
        private static Cert _encryptCert = null;
        //5.1.0验签证书的根证书
        private static X509Certificate _rootCert = null;
        //5.1.0验签证书的中级证书
        private static X509Certificate _middleCert = null;
        //5.1.0验签证书，key是应答的证书的base64
        private static readonly Dictionary<string, X509Certificate> ValidateCerts = new Dictionary<string, X509Certificate>();
        /// <summary>
        /// 银联公司全称
        /// </summary>
        private static readonly string UNIONPAY_CNNAME = "中国银联股份有限公司";

        static CertUtil()
        {
            InitCerCerts();
            InitEncryptCert();
            InitMiddleCert();
            InitRootCert();
            InitSignCert(SdkConfig.SignCertPath, SdkConfig.SignCertPwd);
        }

        /// <summary>
        /// 初始化签名证书
        /// </summary>
        /// <param name="certPath">证书路径</param>
        /// <param name="certPwd">密码</param>
        private static void InitSignCert(string certPath, string certPwd)
        {
            Log.Info("读取签名证书……，地址：" + certPath);

            FileStream fileStream = null;
            try
            {
                if (File.Exists(certPath))
                {
                    Cert signCert = new Cert();
                    using (fileStream = new FileStream(certPath, FileMode.Open))
                    {
                        Pkcs12Store store = new Pkcs12Store(fileStream, certPwd.ToCharArray());

                        string pName = null;
                        foreach (string n in store.Aliases)
                        {
                            if (store.IsKeyEntry(n))
                            {
                                pName = n;
                                //break;
                            }
                        }

                        signCert.AsymmetricKey = store.GetKey(pName).Key;
                        X509CertificateEntry[] chain = store.GetCertificateChain(pName);
                        signCert.X509Certificate = chain[0].Certificate;
                        signCert.CertId = signCert.X509Certificate.SerialNumber.ToString();

                        SignCerts[certPath] = signCert;
                    }

                    Log.Info("签名证书读取成功，序列号：" + signCert.CertId);
                }
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
        }

        /// <summary>
        /// 初始化加密证书
        /// </summary>
        private static void InitEncryptCert()
        {
            if (SdkConfig.EncryptCert == null)
            {
                Log.Info("未配置加密证书路径，不做初始化。");
                return;
            }
            Log.Info("读取加密证书……");

            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(SdkConfig.EncryptCert, FileMode.Open);
                X509Certificate cert = new X509CertificateParser().ReadCertificate(fileStream);

                _encryptCert = new Cert();
                _encryptCert.X509Certificate = cert;
                _encryptCert.CertId = cert.SerialNumber.ToString();
                _encryptCert.AsymmetricKey = cert.GetPublicKey();

                Log.Info("加密证书读取成功，序列号：" + _encryptCert.CertId);
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
        }

        /// <summary>
        /// 初始化中级证书
        /// </summary>
        private static void InitMiddleCert()
        {
            if (SdkConfig.MiddleCertPath == null)
            {
                Log.Info("未配置中级证书路径，不做初始化。");
                return;
            }
            Log.Info("读取中级证书……");

            if (SdkConfig.MiddleCertPath == null || !File.Exists(SdkConfig.MiddleCertPath))
            {
                Log.Error("middleCertPath为空或路径不存在，请检查配置文件middleCertPath的配置情况。");
                return;
            }
            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(SdkConfig.MiddleCertPath, FileMode.Open);
                _middleCert = new X509CertificateParser().ReadCertificate(fileStream);
                Log.Info("中级证书读取成功。");
            }
            catch (Exception e)
            {
                Log.Error("中级证书读取失败：", e);
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
        }

        /// <summary>
        /// 初始化根证书
        /// </summary>
        private static void InitRootCert()
        {
            if (SdkConfig.RootCertPath == null)
            {
                Log.Info("未配置根证书路径，不做初始化。");
                return;
            }
            Log.Info("读取根证书……");

            if (SdkConfig.RootCertPath == null || !File.Exists(SdkConfig.RootCertPath))
            {
                Log.Error("rootCertPath为空或路径不存在，请检查配置文件rootCertPath的配置情况。");
                return;
            }
            FileStream fileStream = null;
            try
            {
                fileStream = new FileStream(SdkConfig.RootCertPath, FileMode.Open);
                _rootCert = new X509CertificateParser().ReadCertificate(fileStream);
                Log.Info("根证书读取成功。");
            }
            catch (Exception e)
            {
                Log.Error("根证书读取失败：", e);
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
        }

        /// <summary>
        /// 获取签名证书私钥
        /// </summary>
        /// <returns></returns>
        public static AsymmetricKeyParameter GetSignKeyFromPfx()
        {
            Log.Debug("读取配置文件证书");
            return GetSignKeyFromPfx(SdkConfig.SignCertPath, SdkConfig.SignCertPwd);
        }

        /// <summary>
        /// 获取签名证书私钥
        /// </summary>
        /// <returns></returns>
        public static AsymmetricKeyParameter GetSignKeyFromPfx(string certPath, string certPwd)
        {
            Log.Debug("传入证书路径获取私钥。");
            if (!SignCerts.ContainsKey(certPath))
            {
                InitSignCert(certPath, certPwd);
            }
            return SignCerts[certPath].AsymmetricKey;
        }

        /// <summary>
        /// 获取签名证书的证书序列号
        /// </summary>
        /// <returns></returns>
        public static string GetSignCertId(string certPath, string certPwd)
        {
            Log.Debug("传入证书路径获取certId。");
            if (!SignCerts.ContainsKey(certPath))
            {
                InitSignCert(certPath, certPwd);
            }
            return SignCerts[certPath].CertId;
        }

        /// <summary>
        /// 获取签名证书的证书序列号
        /// </summary>
        /// <returns></returns>
        public static string GetSignCertId()
        {
            Log.Debug("取配置文件证书");
            return GetSignCertId(SdkConfig.SignCertPath, SdkConfig.SignCertPwd);
        }

        /// <summary>
        /// 验证验签证书
        /// </summary>
        /// <param name="signPubKeyCert"></param>
        /// <returns></returns>
        public static X509Certificate VerifyAndGetPubKey(string signPubKeyCert)
        {
            if (!ValidateCerts.ContainsKey(signPubKeyCert))
            {
                Log.Info("开始处理signPubKeyCert。");
                X509Certificate x509Cert = GetPubKeyCert(signPubKeyCert);
                if (x509Cert == null)
                {
                    return null;
                }
                if (VerifyCertificate(x509Cert))
                {
                    ValidateCerts.Add(signPubKeyCert, x509Cert);
                }
                else
                {
                    Log.Error("验证验签证书失败。");
                    return null;
                }
            }
            return ValidateCerts[signPubKeyCert];
        }

        /// <summary>
        /// 获取公钥
        /// </summary>
        /// <param name="pubKeyCert"></param>
        /// <returns></returns>
        public static X509Certificate GetPubKeyCert(string pubKeyCert)
        {
            try
            {
                pubKeyCert = pubKeyCert.Replace("-----END CERTIFICATE-----", "").Replace("-----BEGIN CERTIFICATE-----", "");
                byte[] x509CertBytes = Convert.FromBase64String(pubKeyCert);
                X509CertificateParser cf = new X509CertificateParser();
                X509Certificate x509Cert = cf.ReadCertificate(x509CertBytes);
                return x509Cert;
            }
            catch (Exception e)
            {
                Log.Error("convert pubKeyCert failed.", e);
                return null;
            }
        }

        private static Boolean VerifyCertificateChain(X509Certificate cert)
        {
            if (null == cert)
            {
                Log.Error("cert must Not null");
                return false;
            }
            X509Certificate rootCert = GetRootCert();
            if (null == rootCert)
            {
                Log.Error("rootCert must Not null");
                return false;
            }
            X509Certificate middleCert = GetMiddleCert();
            if (null == middleCert)
            {
                Log.Error("middleCert must Not null");
                return false;
            }

            try
            {
                X509CertStoreSelector selector = new X509CertStoreSelector();
                selector.Subject = cert.SubjectDN;

                ISet trustAnchors = new HashSet();
                trustAnchors.Add(new TrustAnchor(rootCert, null));
                PkixBuilderParameters pkixParams = new PkixBuilderParameters(trustAnchors, selector);

                IList intermediateCerts = new ArrayList();
                intermediateCerts.Add(rootCert);
                intermediateCerts.Add(middleCert);
                intermediateCerts.Add(cert);

                pkixParams.IsRevocationEnabled = false;

                IX509Store intermediateCertStore = X509StoreFactory.Create("Certificate/Collection",new X509CollectionStoreParameters(intermediateCerts));
                pkixParams.AddStore(intermediateCertStore);

                PkixCertPathBuilder pathBuilder = new PkixCertPathBuilder();
                PkixCertPathBuilderResult result = pathBuilder.Build(pkixParams);
                PkixCertPath path = result.CertPath;

                Log.Info("verify certificate chain succeed." + path);

                return true;
            }
            catch (PkixCertPathBuilderException e)
            {
                Log.Error("verify certificate chain fail.", e);
            }
            catch (Exception e)
            {
                Log.Error("verify certificate chain exception: ", e);
            }
            return false;
        }

        private static Boolean VerifyCertificate(X509Certificate x509Cert)
        {
            string cn = GetIdentitiesFromCertficate(x509Cert);
            try
            {
                x509Cert.CheckValidity();//验证有效期
                                         // x509Cert.Verify(rootCert.GetPublicKey());
                if (!VerifyCertificateChain(x509Cert))  //验证书链
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Log.Error("verifyCertificate fail", e);
                return false;
            }

            if (!"false".Equals(SdkConfig.IfValidateCNName))
            {
                // 验证公钥是否属于银联
                if (!UNIONPAY_CNNAME.Equals(cn))
                {
                    Log.Error("cer owner is not CUP:" + cn);
                    return false;
                }
            }
            else
            {
                if (!UNIONPAY_CNNAME.Equals(cn) && !"00040000:SIGN".Equals(cn))
                {
                    Log.Error("cer owner is not CUP:" + cn);
                    return false;
                }
            }
            return true;
        }

        private static string GetIdentitiesFromCertficate(X509Certificate aCert)
        {
            string tDn = aCert.SubjectDN.ToString();
            string tPart = "";
            if (!string.IsNullOrEmpty(tDn))
            {
                string[] tSplitStr = tDn.Substring(tDn.IndexOf("CN=", StringComparison.Ordinal)).Split("@".ToCharArray());
                if (tSplitStr.Length > 2 && tSplitStr[2] != null)
                    tPart = tSplitStr[2];
            }
            return tPart;
        }

        /// <summary>
        /// 获取加密证书的证书序列号
        /// </summary>
        /// <returns></returns>
        public static string GetEncryptCertId()
        {
            if (_encryptCert == null)
            {
                InitEncryptCert();
            }
            if (_encryptCert != null) return _encryptCert.CertId;
            return "";
        }
        
        /// <summary>
        /// 获取加密证书的证书序列号
        /// </summary>
        /// <returns></returns>
        private static X509Certificate GetRootCert()
        {
            if (_rootCert == null)
            {
                InitRootCert();
            }
            return _rootCert;
        }

        /// <summary>
        /// 获取加密证书的证书序列号
        /// </summary>
        /// <returns></returns>
        private static X509Certificate GetMiddleCert()
        {
            if (_middleCert == null)
            {
                InitMiddleCert();
            }
            return _middleCert;
        }

        /// <summary>
        /// 获取加密证书的RSACryptoServiceProvider
        /// </summary>
        /// <returns></returns>
        public static AsymmetricKeyParameter GetEncryptKey()
        {
            if (_encryptCert == null)
            {
                InitEncryptCert();
            }
            return _encryptCert.AsymmetricKey;
        }

        private static void InitCerCerts()
        {
            if (SdkConfig.ValidateCertDir == null)
            {
                Log.Info("未配置验签证书路径，不做初始化。");
                return;
            }
            Log.Info("读取验签证书文件夹下所有cer文件……");
            DirectoryInfo directory = new DirectoryInfo(SdkConfig.ValidateCertDir);
            FileInfo[] files = directory.GetFiles("*.cer");
            if (0 == files.Length)
            {
                Log.Info("请确定[" + SdkConfig.ValidateCertDir + "]路径下是否存在cer文件");
                return;
            }
            foreach (FileInfo file in files)
            {
                FileStream fileStream = null;
                try
                {
                    using (fileStream = new FileStream(file.DirectoryName + "\\" + file.Name, FileMode.Open))
                    {
                        X509Certificate certificate = new X509CertificateParser().ReadCertificate(fileStream);

                        Cert cert = new Cert
                        {
                            X509Certificate = certificate,
                            CertId = certificate.SerialNumber.ToString(),
                            AsymmetricKey = certificate.GetPublicKey()
                        };
                        CerCerts[cert.CertId] = cert;

                        Log.Info(file.Name + "读取成功，序列号：" + cert.CertId);
                    }
                }
                finally
                {
                    if (fileStream != null)
                        fileStream.Close();
                }
            }
        }

        /// <summary>
        /// 通过证书id，获取验证签名的证书
        /// </summary>
        /// <param name="certId"></param>
        /// <returns></returns>
        public static AsymmetricKeyParameter GetValidateKeyFromPath(string certId)
        {
            if (CerCerts == null || CerCerts.Count <= 0)
            {
                InitCerCerts();
            }
            if (CerCerts == null || CerCerts.Count <= 0)
            {
                Log.Info("未读取到任何证书……");
                return null;
            }
            if (CerCerts.ContainsKey(certId))
            {
                return CerCerts[certId].AsymmetricKey;
            }
            else
            {
                Log.Info("未匹配到序列号为[" + certId + "]的证书");
                return null;
            }
        }

        public static void ResetEncryptCertPublicKey()
        {
            _encryptCert = null;
            InitEncryptCert();
        }
    }
}