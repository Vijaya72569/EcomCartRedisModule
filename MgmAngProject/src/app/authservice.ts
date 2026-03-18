import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Cartservice } from './cartservice';

@Injectable({
  providedIn: 'root',
})
export class Authservice {
 // apiUrl="https://localhost:7067/api/Users";
  apiUrl="http://localhost:5072/api/Users";
  constructor(private http:HttpClient,private router:Router,private cartservice:Cartservice)
  {}
  login(data:any){
    return this.http.post(this.apiUrl+"/login",data)
    
  }
  signin(data:any){
    return this.http.post(this.apiUrl+"/register",data)
  }
   logout(){

    // remove login data
    localStorage.removeItem("token");
    localStorage.removeItem("user");

    // optional
    //localStorage.removeItem("cart");
    //console.log("remove localstorage");
     // ----------- MODIFIED -----------
    // clear cart service state
    this.cartservice.clearCart();
    // ----------- END ----------------


    alert("Logged Out Successfully");

    this.router.navigate(['/login']);
  }

  // ----------------------------
// Merge Guest Cart → Redis Cart
// ----------------------------
/*
mergeGuestCart(userId:number)
{

  const cart = localStorage.getItem("cart");

  // if no guest cart
  if(!cart) return;

  const items = JSON.parse(cart);

  const request = {
    userId: userId,
    items: items.map((x:any) => ({
      productId: x.image.id,
      price: x.image.price,
      quantity: x.quantity
    }))
  };

  this.http.post("http://localhost:5072/api/Cart/merge", request)
  .subscribe((res:any)=>{

    console.log("Cart merged",res);

    // remove guest cart after merge
    localStorage.removeItem("cart");

    // reload Redis cart
    this.cartservice.loadRedisCart();

  });

}
  */
 mergeGuestCart(userId:number){

  const cart = localStorage.getItem("cart");

  if(!cart) return;

  const items = JSON.parse(cart);

  if(items.length === 0) return;

  const request = {
    userId:userId,
    items:items.map((x:any)=>({
      productId:x.image.id,
      price:x.image.price,
      quantity:x.quantity
    }))
  };

  console.log(request);

  this.http.post("http://localhost:5072/api/Cart/merge",request)
  .subscribe(res=>{

    console.log("Cart merged",res);

    localStorage.removeItem("cart");

    this.cartservice.loadRedisCart();

  });

}
}
