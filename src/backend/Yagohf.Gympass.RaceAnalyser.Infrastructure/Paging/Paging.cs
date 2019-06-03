namespace Yagohf.Gympass.RaceAnalyser.Infrastructure.Paging
{
    public class Paging
    {
        private readonly int _itemsPerPage;
        private readonly int _totalItems;
        private readonly int _currentPage;

        public Paging(int currentPage, int totalItems, int itemsPerPage)
        {
            this._totalItems = totalItems;
            this._currentPage = currentPage;
            this._itemsPerPage = itemsPerPage;
        }

        public int CurrentPage
        {
            get
            {
                return this._currentPage;
            }
        }

        public int ItemsPerPage
        {
            get
            {
                return this._itemsPerPage;
            }
        }

        public int TotalItems
        {
            get
            {
                return this._totalItems;
            }
        }
    }
}
