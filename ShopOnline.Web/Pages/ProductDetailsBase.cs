using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public class ProductDetailsBase : ComponentBase
    {
        [Parameter]
        public int Id { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }

        [Inject]
        public IManageCartItemsLocalStorageService ManageCartItemsLocalStorageService { get; set; }

        [Inject]
        public NavigationManager NavigationManager { get; set; }
        public ProductDto Product { get; set; }

        public string ErrorMessage { get; set; }

        private List<CartItemDto> shoppingCartItems { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                shoppingCartItems = await ManageCartItemsLocalStorageService.GetCollection();
                Product = await GetProductById(Id);
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task AddToCart_Click(CartItemToAddDto cartItemToAddDto)
        {
            try
            {
                var cartItemDto = await ShoppingCartService.AddItem(cartItemToAddDto);

                if (cartItemDto != null)
                {
                    shoppingCartItems.Add(cartItemDto);
                    await ManageCartItemsLocalStorageService.SaveCollection(shoppingCartItems);
                }
                NavigationManager.NavigateTo("/ShoppingCart");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<ProductDto> GetProductById(int Id)
        {
            var productsDtos = await ManageProductsLocalStorageService.GetCollection();

            if (productsDtos != null)
            {
                return productsDtos.SingleOrDefault(p => p.Id == Id);
            }

            return null;
        }
    }
}
