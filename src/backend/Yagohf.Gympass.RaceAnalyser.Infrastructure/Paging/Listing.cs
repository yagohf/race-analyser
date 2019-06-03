using System.Collections.Generic;

namespace Yagohf.Gympass.RaceAnalyser.Infrastructure.Paging
{
    public class Listing<T>
    {
        private readonly IEnumerable<T> _list;
        private readonly Paging _paging;

        public Listing(IEnumerable<T> list) : this(list, null)
        {

        }

        public Listing(IEnumerable<T> list, Paging paging)
        {
            this._list = list;
            this._paging = paging;
        }

        public IEnumerable<T> List { get { return this._list; } }
        public Paging Paging { get { return this._paging; } }
    }
}
