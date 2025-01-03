namespace Shopping_Tutorial.Models
{
    public class PhanTrang
    {
        public int TotalItems { get;private set; } //tổng số item

        public int PageSize { get; private set; } //tổng sp/trang

        public int CurentPage { get; private set; } //trang hien tai

        public int TotalPages { get; private set; } //tong trang

        public int StartPage { get; private set; } //trang bắt đầu

        public int EndPage { get; private set; } //trang kết thúc

        public PhanTrang() { }

        public PhanTrang(int totalItems, int page, int pageSize = 10)
        {
            //Làm tròn tổng items/10
            int totalPages = (int)Math.Ceiling((decimal)totalItems/(decimal)pageSize);

            int currentPage = page; //page hiện tại bằng 1

            int startPage = currentPage - 5;
            int endPage = currentPage + 4;

            if(startPage <= 0)
            {
                //nếu trang bắt đầu <=0 thì trang cối sẽ set là =10 và trang bắt đầu sẽ là 1 sp
                endPage = endPage-(startPage-1);
                startPage = 1;

            }
            if(endPage > totalPages)
            {
                endPage -= totalPages;
                if(endPage >10) {
                    startPage = endPage - 9;
                }
            }
            TotalItems = totalItems;
            CurentPage = currentPage;
            PageSize = pageSize;
            TotalPages = totalPages;
            StartPage = startPage;
            EndPage = endPage;
        }
    }
}
