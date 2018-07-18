using System;
using log4net;

namespace ChinaPayment
{
    public class LogHelper
    {
        /// <summary>
        /// 获取日志对象
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ILog GetLogger(Type type)
        {
            return LogManager.GetLogger(type);
        }

        /**
         * 向日志文件写入调试信息
         * @param className 类名
         * @param content 写入内容
         */
        public static void Debug(string className, string content)
        {
            ILog _log = LogManager.GetLogger(className);

            _log.Debug(className + "  " + content);
        }

        /**
        * 向日志文件写入运行时信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Info(string className, string content)
        {
            ILog _log = LogManager.GetLogger(className);

            _log.Info(className + "  " + content);
        }

        /**
        * 向日志文件写入出错信息
        * @param className 类名
        * @param content 写入内容
        */
        public static void Error(string className, string content)
        {
            ILog _log = LogManager.GetLogger(className);

            _log.Error(className + "  " + content);
        }
    }
}