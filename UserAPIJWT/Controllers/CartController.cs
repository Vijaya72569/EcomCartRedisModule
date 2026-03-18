using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;
using UserAPIJWT.Models;

namespace UserAPIJWT.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IDistributedCache _cache;

        public CartController(IDistributedCache cache)
        {
            _cache = cache;
        }

        //[HttpPost("add")]
        //public async Task<IActionResult> AddToCart(AddCartRequest request)
        //{
        //    string key = $"cart:{request.UserId}";

        //    var existing = await _cache.GetStringAsync(key);

        //    List<CartItems> cart;

        //    if (existing != null)
        //        cart = JsonSerializer.Deserialize<List<CartItems>>(existing);
        //    else
        //        cart = new List<CartItems>();

        //    cart.Add(request.Item);

        //    var json = JsonSerializer.Serialize(cart);

        //    await _cache.SetStringAsync(key, json);

        //    return Ok(cart);
        //}

        //[HttpPost("add")]
        //public async Task<IActionResult> AddCart([FromBody] CartItems data)
        //{
        //    string key = $"cart:{data.UserId}";

        //    var cart = await _cache.GetStringAsync(key);

        //    List<CartItems> items;

        //    if (string.IsNullOrEmpty(cart))
        //    {
        //        items = new List<CartItems>();
        //    }
        //    else
        //    {
        //        items = JsonSerializer.Deserialize<List<CartItems>>(cart);
        //    }

        //    items.Add(data);

        //    await _cache.SetStringAsync(key, JsonSerializer.Serialize(items));

        //    return Ok(items);
        //}


        //[HttpGet("{userId}")]
        //public async Task<IActionResult> GetCart(int userId)
        //{
        //    string key = $"cart:{userId}";

        //    var cart = await _cache.GetStringAsync(key);

        //    if (cart == null)
        //        return Ok(new List<CartItems>());

        //    return Ok(JsonSerializer.Deserialize<List<CartItems>>(cart));
        //}


        //[HttpDelete("{userId}")]
        //public async Task<IActionResult> ClearCart(int userId)
        //{
        //    string key = $"cart:{userId}";

        //    await _cache.RemoveAsync(key);

        //    return Ok("Cart Cleared");
        //}

        // ---------------- ADD ITEM ----------------
        [HttpPost("add")]
        public async Task<IActionResult> AddCart([FromBody] CartItems data)
        {
            string key = $"cart:{data.UserId}";

            var cartJson = await _cache.GetStringAsync(key);

            Dictionary<int, int> cart;

            if (string.IsNullOrEmpty(cartJson))
                cart = new Dictionary<int, int>();
            else
                cart = JsonSerializer.Deserialize<Dictionary<int, int>>(cartJson)!;

            // -------- INCREASE QUANTITY --------
            if (cart.ContainsKey(data.ProductId))
                cart[data.ProductId] += 1;
            else
                cart[data.ProductId] = 1;
            // ----------------------------------

            await _cache.SetStringAsync(key,
                JsonSerializer.Serialize(cart));

            return Ok(cart);
        }

        // ---------------- GET CART ----------------
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetCart(int userId)
        {
            string key = $"cart:{userId}";

            var cartJson = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(cartJson))
                return Ok(new Dictionary<int, int>());

            return Ok(JsonSerializer.Deserialize<Dictionary<int, int>>(cartJson));
        }

        // ---------------- REMOVE ITEM ----------------
        //[HttpDelete("{userId}/{productId}")]
        //public async Task<IActionResult> RemoveItem(int userId, int productId)
        //{
        //    string key = $"cart:{userId}";

        //    var cartJson = await _cache.GetStringAsync(key);

        //    if (string.IsNullOrEmpty(cartJson))
        //        return Ok();

        //    var cart = JsonSerializer.Deserialize<Dictionary<int, int>>(cartJson);

        //    cart.Remove(productId);

        //    await _cache.SetStringAsync(key,
        //        JsonSerializer.Serialize(cart));

        //    return Ok(cart);
        //}
        [HttpDelete("remove/{userId}/{productId}")]
        public async Task<IActionResult> RemoveItem(int userId, int productId)
        {
            string key = $"cart:{userId}";

            var cartJson = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(cartJson))
                return Ok();

            var cart = System.Text.Json.JsonSerializer
                .Deserialize<Dictionary<int, int>>(cartJson)!;

            cart.Remove(productId);

            await _cache.SetStringAsync(key,
                System.Text.Json.JsonSerializer.Serialize(cart));

            return Ok(cart);
        }

        // update quantity
        [HttpPut("update")]
        public async Task<IActionResult> UpdateQuantity([FromBody] CartItems data)
        {
            string key = $"cart:{data.UserId}";

            var cartJson = await _cache.GetStringAsync(key);

            if (string.IsNullOrEmpty(cartJson))
                return Ok();

            var cart = System.Text.Json.JsonSerializer
                .Deserialize<Dictionary<int, int>>(cartJson)!;

            if (cart.ContainsKey(data.ProductId))
            {
                cart[data.ProductId] = data.Quantity;
            }

            await _cache.SetStringAsync(key,
                System.Text.Json.JsonSerializer.Serialize(cart));

            return Ok(cart);
        }
        //-- merge cart
        //[HttpPost("merge")]
        //public async Task<IActionResult> MergeCart([FromBody] MergeCartRequest request)
        //{
        //    if (request == null || request.Items == null)
        //        return BadRequest("Invalid cart data");

        //    string key = $"cart:{request.UserId}";

        //    var existingData = await _cache.GetStringAsync(key);

        //    List<CartItem> redisCart;

        //    if (existingData == null)
        //        redisCart = new List<CartItem>();
        //    else
        //        redisCart = JsonSerializer.Deserialize<List<CartItem>>(existingData)!;

        //    foreach (var item in request.Items)
        //    {
        //        var existingItem = redisCart
        //            .FirstOrDefault(x => x.ProductId == item.ProductId);

        //        if (existingItem != null)
        //            existingItem.Quantity += item.Quantity;
        //        else
        //            redisCart.Add(item);
        //    }

        //    await _cache.SetStringAsync(
        //        key,
        //        JsonSerializer.Serialize(redisCart)
        //    );

        //    return Ok(redisCart);
        //}

        [HttpPost("merge")]
        public async Task<IActionResult> MergeCart([FromBody] MergeCartRequest request)
        {
            if (request == null || request.Items == null)
                return BadRequest("Invalid cart data");

            string key = $"cart:{request.UserId}";

            var cartJson = await _cache.GetStringAsync(key);

            Dictionary<int, int> cart;

            if (string.IsNullOrEmpty(cartJson))
                cart = new Dictionary<int, int>();
            else
                cart = JsonSerializer.Deserialize<Dictionary<int, int>>(cartJson)!;

            foreach (var item in request.Items)
            {
                if (cart.ContainsKey(item.ProductId))
                    cart[item.ProductId] += item.Quantity;
                else
                    cart[item.ProductId] = item.Quantity;
            }

            await _cache.SetStringAsync(
                key,
                JsonSerializer.Serialize(cart)
            );

            return Ok(cart);
        }
    }
}
