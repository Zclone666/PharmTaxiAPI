using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PharmTaxiAPI.Methods
{
    public class Logger : IDisposable
    {
        private string _logFileName = null;
        private StreamWriter _log = null;
        private Task _lastTask = null;
        private bool _dontKeepFileOpen;
        private bool _append;
        private Mutex _logMutex = null;

        public Logger(Type owner, bool append, bool dontKeepFileOpen)
        {
            _dontKeepFileOpen = dontKeepFileOpen;
            _append = append;
            _logFileName = CreateFileName(owner);

            if (_dontKeepFileOpen)
            {
                string mutexName = String.Concat(_logFileName.Replace('\\', '_'), "_Mutex");
                //mutexName = mutexName.Replace('/', '\\');

                if (mutexName.Length > 260)
                    mutexName = mutexName.Substring(mutexName.Length - 260);

                //немного доработал, типа зачем строить логику на исключения если можно тоже самое на условиях сделать
                #region старое
                //try
                //{
                //    _logMutex = Mutex.OpenExisting("log.txt");
                //    //_logMutex = Mutex.OpenExisting(mutexName);
                //}
                //catch (WaitHandleCannotBeOpenedException) // Выпадет если он ещё не создан
                //{
                //    try
                //    {
                //        _logMutex = new Mutex(false, "log.txt");

                //        //_logMutex = new Mutex(false, mutexName);
                //    }
                //    catch (WaitHandleCannotBeOpenedException) // Выпадет если другой поток уже успел его создать
                //    {
                //        //_logMutex = Mutex.OpenExisting(mutexName);
                //        _logMutex = Mutex.OpenExisting("log.txt");
                //    }
                //}
                #endregion

                if (File.Exists("log.txt"))
                {
                    _logMutex = Mutex.OpenExisting("log.txt");
                }
                else
                {
                    try
                    {
                        _logMutex = new Mutex(false, "log.txt");

                        //_logMutex = new Mutex(false, mutexName);
                    }
                    catch (WaitHandleCannotBeOpenedException) // Выпадет если другой поток уже успел его создать
                    {
                        //_logMutex = Mutex.OpenExisting(mutexName);
                        _logMutex = Mutex.OpenExisting("log.txt");
                    }
                }
            }
            else
            {
                try
                {
                    _log = new StreamWriter(_logFileName, _append, Encoding.UTF8);
                }
                catch (IOException) // Вторая попытка (файл занят другим процессом)
                {
                    Thread.Sleep(100);

                    try
                    {
                        _log = new StreamWriter(_logFileName, _append, Encoding.UTF8);
                    }
                    catch (IOException)
                    {
                        // Если снова упало - создаём с уникальным именем
                        _logFileName = Path.Combine(Path.GetDirectoryName(_logFileName), String.Format("{0}_{1}{2}", Path.GetFileNameWithoutExtension(_logFileName), Guid.NewGuid(), Path.GetExtension(_logFileName)));
                        _log = new StreamWriter(_logFileName, _append, Encoding.UTF8);
                    }
                }
            }
        }

        #region IDisposable members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        protected virtual string GetLogsFolder()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
            Directory.CreateDirectory(path);
            return (path);
        }

        protected virtual string CreateFileName(Type owner)
        {
            return (Path.Combine(GetLogsFolder(), String.Format("{0}Log.txt", owner.Name)));
        }

        protected virtual void Dispose(bool clearAll)
        {
            if (_lastTask != null)
                _lastTask.Wait();

            if (_log != null)
                _log.Dispose();

            // НЕ УДАЛЯЕМ!!! Мутекс межпроцессовый, так что пускай себе висит в памяти хоть до опупения
            //if (_logMutex != null)
            //	_logMutex.Dispose();
        }

        public string NewLine
        {
            get
            {
                return ((_log == null) ? Environment.NewLine : _log.NewLine);
            }
        }

        public void WriteLine()
        {
            if (_dontKeepFileOpen)
            {
                _logMutex.WaitOne();
                try
                {
                    using (StreamWriter sw = new StreamWriter(_logFileName, _append, Encoding.UTF8))
                    {
                        sw.WriteLine();
                    }
                }
                catch
                {
                }
                finally
                {
                    _logMutex.ReleaseMutex();
                }
            }
            else
            {
                if (_lastTask != null)
                    _lastTask.Wait();

                _lastTask = _log.WriteLineAsync();
            }
        }

        public void WriteLine(char c)
        {
            if (_dontKeepFileOpen)
            {
                _logMutex.WaitOne();
                try
                {
                    using (StreamWriter sw = new StreamWriter(_logFileName, _append, Encoding.UTF8))
                    {
                        sw.WriteLine(c);
                    }
                }
                catch
                {
                }
                finally
                {
                    _logMutex.ReleaseMutex();
                }
            }
            else
            {
                if (_lastTask != null)
                    _lastTask.Wait();

                _lastTask = _log.WriteLineAsync(c);
            }
        }

        public void WriteLine(string text)
        {
            if (_dontKeepFileOpen)
            {
                _logMutex.WaitOne();
                try
                {
                    using (StreamWriter sw = new StreamWriter(_logFileName, _append, Encoding.UTF8))
                    {
                        sw.WriteLine(text);
                    }
                }
                catch
                {
                }
                finally
                {
                    _logMutex.ReleaseMutex();
                }
            }
            else
            {
                if (_lastTask != null)
                    _lastTask.Wait();

                _lastTask = _log.WriteLineAsync(text);
            }
        }

        public void WriteLine(string format, object arg0)
        {
            WriteLine(String.Format(format, arg0));
        }

        public void WriteLine(string format, params object[] arg)
        {
            WriteLine(String.Format(format, arg));
        }

        public void WriteLine(string format, object arg0, object arg1)
        {
            WriteLine(String.Format(format, arg0, arg1));
        }

        public void WriteLine(string format, object arg0, object arg1, object arg2)
        {
            WriteLine(String.Format(format, arg0, arg1, arg2));
        }

        public void WriteError(Exception err)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0:dd.MM.yyyy HH:mm:ss} ERROR !!!!!!!!!!!!!!!!!!!!!!!{1}{1}", DateTime.Now, NewLine);

            for (Exception e = err; e != null; e = e.InnerException)
                sb.AppendFormat("{0}: {1}{3}{2}{3}{3}", e.GetType().Name, e.Message, e.StackTrace, NewLine);

            WriteLine(sb.ToString());
        }

        public void WriteError(Exception err, string methodName)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0:dd.MM.yyyy HH:mm:ss} ERROR in {1} !!!!!!!!!!!!!!!!!!!!!!!{2}{2}", DateTime.Now, methodName, NewLine);

            for (Exception e = err; e != null; e = e.InnerException)
                sb.AppendFormat("{0}: {1}{3}{2}{3}{3}", e.GetType().Name, e.Message, e.StackTrace, NewLine);

            WriteLine(sb.ToString());
        }
    }

    public class Logs
    {
        public Logger _log;
        public Logs()
        {
            _log = new Logger(GetType(), true, true);
        }
        public void Log(string method, string message)
        {
            _log.WriteLine("============================================={3}MESSAGE{3}At:      {0:G}{3}From:    {1} ==>> {4}{3}{2}{3}",
                        DateTime.Now,
                        GetType().Name,
                        message,
                        _log.NewLine,
                        method);
        }
    }
}
