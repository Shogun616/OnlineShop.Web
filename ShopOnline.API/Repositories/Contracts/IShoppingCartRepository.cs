using ShopOnline.API.Entities;
using ShopOnline.Models.Dtos;

namespace ShopOnline.API.Repositories.Contracts
{
    public interface IShoppingCartRepository
    {
        Task<CartItem> AddItem(CartItemToAddDto cartItemToAdd);
        Task<CartItem> UpdateQty(int Id, CartItemQtyUpdateDto cartItemQtyUpdate);
        Task<CartItem> DeleteItem(int Id);
        Task<CartItem> GetItem(int Id);
        Task<IEnumerable<CartItem>> GetItems(int userId);
    }
}
