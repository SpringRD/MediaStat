using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MediaStat.Data.Services
{
    public class AppState
    {
        public static AppState Instance { get; set; }
        public AppState()
        {
            Instance = this;
        }
        private static Dictionary<String, bool> Data { get; set; } = new Dictionary<String, bool>();
        private bool _isAdmin;
        public event Action OnChange;
        public static bool IsAdmin (String user)
        {
            return Data.ContainsKey(user) && Data[user];
        }

        public static void PutData(String user, bool isAdmin)
        {

            Data[user] = isAdmin;
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
