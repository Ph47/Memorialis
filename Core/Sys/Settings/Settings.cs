using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memorialis.Core.Sys.Settings
{
    public sealed class Settings
    {
        //private Dictionary<SettingGroup, Dictionary<string, string>> cache;
        private List<Setting> settings;

        #region Singleton
        private static volatile Settings instance;
        private static object syncRoot = new Object();        

        public static Settings Current
        {
            get
            {
                if (instance == null)
                {
                    lock (syncRoot)
                    {
                        if (instance == null)
                            instance = new Settings();
                    }
                }

                return instance;
            }
        }
        #endregion

        private Settings()
        {
            using (Context db = new Context())
            {
                settings = db.Settings.ToList();
            }
        }

        public void Save()
        {
            using (Context db = new Context())
            {
                foreach (Setting setting in Settings.Current.settings)
                {
                    db.Settings.Attach(setting);
                    db.Entry(setting).Property("Value").IsModified = true;
                }
                db.SaveChanges();
            }
        }

        public string this[string name]
        {
            get
            {
                return this.Get(name).Value;
            }
            set
            {
                if (value == null)
                    throw new InvalidOperationException("Setting cant be null");
                this.Get(name).Value = value;                
            }
        }

        private Setting Get(string name)
        {
            Setting setting = Settings.Current.settings.Where(s => s.Name == name).FirstOrDefault();
            if (setting == null)
                throw new IndexOutOfRangeException("Incorrect setting name");
            return setting;
        }

        public Dictionary<string,string> Export()
        {
            return 
                settings
                .Where(s=>s.IsPublic)
                .ToDictionary(s => string.Copy(s.Name), s => string.Copy(s.Value));
        }
        //public static string 
    }
}
