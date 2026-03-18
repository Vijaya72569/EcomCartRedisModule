import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { isPlatformBrowser } from '@angular/common';
import { CartItem } from './cartitem';
import { Image } from './imageservice';

@Injectable({
  providedIn: 'root'
})
export class Cartservice {

  private cartItems: CartItem[] = [];
  private isBrowser: boolean;

  private cartCountSubject = new BehaviorSubject<number>(0);
  cartCount$ = this.cartCountSubject.asObservable();

  constructor(
    @Inject(PLATFORM_ID) private platformId: Object,
    private http: HttpClient
  ) {

    this.isBrowser = isPlatformBrowser(this.platformId);

    if (!this.isBrowser) return;

    const token = localStorage.getItem("token");

    if (token) {
      this.loadRedisCart();
    } else {
      this.loadGuestCart();
    }

  }

  // ----------------------------
  // Load Guest Cart
  // ----------------------------
  loadGuestCart(){

    const cart = localStorage.getItem("cart");

    this.cartItems = cart ? JSON.parse(cart) : [];

    this.updateCartCount();

  }

  // ----------------------------
  // Load Redis Cart
  // ----------------------------
  loadRedisCart(){

    const userStr = localStorage.getItem("user");
    if(!userStr) return;

    const user = JSON.parse(userStr);

    this.http.get<any>(`http://localhost:5072/api/Cart/${user.id}`)
    .subscribe(cart => {

      this.cartItems = [];

      Object.keys(cart).forEach(productId => {

        const quantity = cart[productId];

        this.http.get<any>(`http://localhost:5072/api/Product/${productId}`)
        .subscribe(product => {

          this.cartItems.push({
            image:{
              id: product.id,
              url: product.url,
              price: product.price
            },
            quantity: quantity
          });

          this.updateCartCount();

        });

      });

    });

  }

  // ----------------------------
  // Add Item
  // ----------------------------
  addToCart(image: Image){

    const token = localStorage.getItem("token");

    const existing = this.cartItems.find(x => x.image.id === image.id);

    if(existing){
      existing.quantity++;
    }
    else{
      this.cartItems.push({image, quantity:1});
    }

    if(!token){

      // Guest cart
      localStorage.setItem("cart", JSON.stringify(this.cartItems));

    }
    else{

      // Redis cart
      const user = JSON.parse(localStorage.getItem("user")!);

      let data = {
        userId:user.id,
        productId:image.id,
        quantity:1
      };

      this.http.post("http://localhost:5072/api/Cart/add",data)
      .subscribe();

    }

    this.updateCartCount();

  }

  // ----------------------------
  // Increase Quantity
  // ----------------------------
  increaseQuantity(id:number){

    const item = this.cartItems.find(x => x.image.id === id);

    if(!item) return;

    item.quantity++;

    const token = localStorage.getItem("token");

    if(!token){

      localStorage.setItem("cart", JSON.stringify(this.cartItems));

    }
    else{

      const user = JSON.parse(localStorage.getItem("user")!);

      let data = {
        userId:user.id,
        productId:id,
        quantity:item.quantity
      };

      this.http.put("http://localhost:5072/api/Cart/update",data)
      .subscribe();

    }

    this.updateCartCount();

  }

  // ----------------------------
  // Decrease Quantity
  // ----------------------------
  decreaseQuantity(id:number){

    const item = this.cartItems.find(x => x.image.id === id);

    if(!item) return;

    item.quantity--;

    if(item.quantity <=0){
      this.removeFromCart(id);
      return;
    }

    const token = localStorage.getItem("token");

    if(!token){

      localStorage.setItem("cart", JSON.stringify(this.cartItems));

    }
    else{

      const user = JSON.parse(localStorage.getItem("user")!);

      let data = {
        userId:user.id,
        productId:id,
        quantity:item.quantity
      };

      this.http.put("http://localhost:5072/api/Cart/update",data)
      .subscribe();

    }

    this.updateCartCount();

  }

  // ----------------------------
  // Remove Item
  // ----------------------------
  removeFromCart(id:number){

    this.cartItems = this.cartItems.filter(x=>x.image.id !== id);

    const token = localStorage.getItem("token");

    if(!token){

      localStorage.setItem("cart", JSON.stringify(this.cartItems));

    }
    else{

      const user = JSON.parse(localStorage.getItem("user")!);

      this.http.delete(`http://localhost:5072/api/Cart/remove/${user.id}/${id}`)
      .subscribe();

    }

    this.updateCartCount();

  }

  // ----------------------------
  // Get Items
  // ----------------------------
  getCartItems(){

    return [...this.cartItems];

  }

  // ----------------------------
  // Total Price
  // ----------------------------
  getTotalPrice(){

    return this.cartItems.reduce(
      (sum,item)=> sum + (item.image.price * item.quantity),
      0
    );

  }

  // ----------------------------
  // Update Count
  // ----------------------------
  updateCartCount(){

    const total = this.cartItems.reduce(
      (sum,item)=> sum + item.quantity,
      0
    );

    this.cartCountSubject.next(total);

  }

  // ----------------------------
  // Clear Cart (Logout)
  // ----------------------------
  clearCart(){

    this.cartItems = [];

    this.cartCountSubject.next(0);

  }

}