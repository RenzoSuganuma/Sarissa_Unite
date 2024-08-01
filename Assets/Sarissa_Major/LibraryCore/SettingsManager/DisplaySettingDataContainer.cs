using System.Collections.Generic;
using System;

namespace Sarissa
{
    public class DisplaySettingDataContainer
    {
        private Dictionary<string, Tuple<int, int>> _resolutionList = new Dictionary<string, Tuple<int, int>>()
        {
            { "HD", new Tuple<int, int>(1280, 720) },
            { "Full HD", new Tuple<int, int>(1920, 1080) },
            { "UHD", new Tuple<int, int>(3840, 2160) },
        };

        public Dictionary<string, Tuple<int, int>> GetResolutionList
        {
            get { return _resolutionList; }
        }

        private List<string> _refreshRateList = new()
        {
            "60",
            "75",
            "120",
            "144",
            "165"
        };

        public List<string> GetRefreshRateList
        {
            get { return _refreshRateList; }
        }
    }
}