Full Stack E-Commerce Project
Tech Stack

* Frontend: Angular
* Backend: ASP.NET Core Web API
* Authentication: JWT
* Caching: Redis
* Database: SQL Server
----------------------------------------------------------------------------
Project Structure
***MgmAngProject** → Angular Frontend
***UserAPIJWT** → .NET Core API
---------------------------------------------------------------------------
Features
User Authentication (JWT)
Product Listing
Cart Management (LocalStorage + Redis)
API Gateway (if added)
Microservices Architecture
----------------------------------------------------------------------------
Setup Instructions
1.Clone the repo
bash
git clone https://github.com/Vijaya72569/EcomCartRedisModule.git

----------------------------------------------------------------------------
2.Run Backend

* Open `UserAPIJWT`
* Run:

bash
dotnet run
----------------------------------------------------------------------------

3.Run Frontend

bash
cd MgmAngProject
npm install
ng serve
----------------------------------------------------------------------------

API Example
bash
http://localhost:5072/api/Product
http://localhost:5072/api/User/login
http://localhost:5072/api/User/register
http://localhost:5072/api/Cart/CartItems

----------------------------------------------------------------------------
Author

Your Name:K.S.Vijaya
.NET Full Stack Developer  

- GitHub: https://github.com/Vijaya72569  
- Email: vijayaias2017@gmail.com  
