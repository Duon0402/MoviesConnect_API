namespace API.DTOs.Vouchers
{
    public class VoucherOutputDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public decimal Value { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
