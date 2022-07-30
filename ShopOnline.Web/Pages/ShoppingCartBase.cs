using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IJSRuntime Js { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        public List<CartItemDto> ShoppingCartItems { get; set; }

        public string ErrorMessage { get; set; }
        protected string TotalPrice { get; set; }
        protected int TotalQuantity { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                CalculateCartSummaryTotals();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task DeleteCartItem_Click(int Id)
        {
            var cartItemDto = await ShoppingCartService.DeleteItem(Id);

            RemoveCartItem(Id);

            CalculateCartSummaryTotals();
        }

        private CartItemDto GetCartItem(int Id)
        {
            return ShoppingCartItems.FirstOrDefault(i => i.Id == Id);
        }

        protected async Task UpdateQtyCartItem_Click(int Id, int qty)
        {
            try
            {
                if (qty > 0)
                {
                    var updateItemDto = new CartItemQtyUpdateDto
                    {
                        CartItemId = Id,
                        Qty = qty
                    };

                    var returnedUpdateItemDto = await this.ShoppingCartService.UpdateQty(updateItemDto);

                    UpdateItemTotalPrice(returnedUpdateItemDto);

                    CalculateCartSummaryTotals();

                    await MakeUpdateQtyButtonVisible(Id, false);
                }
                else
                {
                    var item = this.ShoppingCartItems.FirstOrDefault(i => i.Id == Id);

                    if (item != null)
                    {
                        item.Qty = 1;
                        item.TotalPrice = item.Price;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void RemoveCartItem(int Id)
        {
            var cartItemDto = GetCartItem(Id);

            ShoppingCartItems.Remove(cartItemDto);
        }

        public void UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);

            if (item != null)
            {
                item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
            }
        }

        public void CalculateCartSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }

        public void SetTotalPrice()
        {
            TotalPrice = this.ShoppingCartItems.Sum(p => p.TotalPrice).ToString("C");
        }

        public void SetTotalQuantity()
        {
            TotalQuantity = this.ShoppingCartItems.Sum(p => p.Qty);
        }

        protected async Task UpdateQty_Input(int Id)
        {
            await MakeUpdateQtyButtonVisible(Id, true);
        }

        private async Task MakeUpdateQtyButtonVisible(int Id, bool visible)
        {
            await Js.InvokeVoidAsync("MakeUpdateQtyButtonVisible", Id, visible);
        }
    }
}
