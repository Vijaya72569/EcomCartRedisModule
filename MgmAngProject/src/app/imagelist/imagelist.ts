/*
import { Component, OnInit } from '@angular/core';
import { Image, Imageservice } from '../imageservice';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Cartservice } from '../cartservice';

@Component({
  selector: 'app-imagelist',
  imports: [FormsModule,CommonModule],
  templateUrl: './imagelist.html',
  styleUrl: './imagelist.css',
})
export class Imagelist implements OnInit{
    images: Image[] = [];


constructor(private imgservice:Imageservice,private cartservice:Cartservice)
{

}
ngOnInit(): void {
  this.imgservice.getImages().subscribe(data => {
     // this.images = data; // this is for apiurl
      this.images = data.images; 
    });
}
   
addToCart(image:Image)
{
 this.cartservice.addToCart(image);
 //console.log(image);
}
}

*/

// change code for productapi
import { Component, OnInit,ChangeDetectorRef } from '@angular/core';
import { Productservice } from '../productservice';
import { Product } from '../product';
import { Cartservice } from '../cartservice';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-imagelist',
  imports: [FormsModule,CommonModule],
  templateUrl: './imagelist.html',
  styleUrl: './imagelist.css',
})
export class Imagelist implements OnInit {

  images:Product[]=[];

  constructor(private productservice:Productservice,private cartservice:Cartservice, private cdr: ChangeDetectorRef){}

  ngOnInit(): void {

    this.productservice.getProducts().subscribe(data=>{
      this.images=data;
       this.cdr.detectChanges();   // 🔹 Fix NG0100 error
    })

  }

  addToCart(product:Product)
  {
     this.cartservice.addToCart(product);
     console.log(product);
  }

}
