using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        public List<CartItemDto> ShoppingCartItems { get; set; }

        public string ErrorMessage { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
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
        }

        private CartItemDto GetCartItem(int Id)
        {
            return ShoppingCartItems.FirstOrDefault(i => i.Id == Id);
        }

        public void RemoveCartItem(int Id)
        {
            var cartItemDto = GetCartItem(Id);

            ShoppingCartItems.Remove(cartItemDto);
        }
    }
}
