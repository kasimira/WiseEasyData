﻿using Core.Models;

namespace Core.Contracts
{
    public interface IIndexAppService
    {
        public IndexAppViewModel GetInfo ();
        public IEnumerable<DataPoint> GetDataPoint ();
    }
}
