import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Product } from './product';

@Injectable({
  providedIn: 'root',
})
export class Productservice {
  apiurl="http://localhost:5072/api/Product";
  constructor(private http:HttpClient){}

   getProducts():Observable<Product[]>
  {
    return this.http.get<Product[]>(this.apiurl)
  }
}
