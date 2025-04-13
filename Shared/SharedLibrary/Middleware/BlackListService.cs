using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Middleware
{
    public class BlackListService : IBlackListService
    {
        private readonly ConcurrentDictionary<string, DateTime> _blacklist = new ConcurrentDictionary<string, DateTime>();

        public void AddTokenToBlacklist(string token)
        {
            _blacklist[token] = DateTime.UtcNow;
        }

        public bool IsTokenBlacklisted(string token)
        {
            return _blacklist.ContainsKey(token);
        }

        public void CleanupExpiredTokens(TimeSpan tokenLifetime)
        {
            var now = DateTime.UtcNow;
            foreach (var token in _blacklist.Keys)
            {
                if (_blacklist.TryGetValue(token, out var addedTime) && now - addedTime > tokenLifetime)
                {
                    _blacklist.TryRemove(token, out _);
                }
            }
        }
    }
}
