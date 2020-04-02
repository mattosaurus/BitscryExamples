using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OData.Models.AdventureWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OData.Controllers
{
    public class ApiController : ODataController
    {
        private readonly ILogger<ApiController> _logger;
        private readonly Models.AdventureWorksContext _context;

        public ApiController(ILogger<ApiController> logger, Models.AdventureWorksContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/addresses
        [HttpGet]
        [EnableQuery(PageSize = 50)]
        [ODataRoute("addresses")]
        public IQueryable<Address> GetAddresses()
        {
            return _context.Addresses;
        }

        // GET: api/addresses(451)
        [HttpGet]
        [EnableQuery]
        [ODataRoute("addresses({id})")]
        public IActionResult GetAddresses([FromODataUri] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var address = SingleResult.Create(_context.Addresses.Where(p => p.AddressId == id));

            return Ok(address);
        }

        // GET: api/customers
        [HttpGet]
        [EnableQuery(PageSize = 50)]
        [ODataRoute("customers")]
        public IQueryable<Customer> GetCustomers()
        {
            return _context.Customers;
        }

        // GET: api/customers(5)
        [HttpGet]
        [EnableQuery]
        [ODataRoute("customers({id})")]
        public IActionResult GetCustomers([FromODataUri] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = SingleResult.Create(_context.Customers.Where(p => p.CustomerId == id));

            return Ok(customer);
        }

        // GET: api/customerAddresses
        [HttpGet]
        [EnableQuery(PageSize = 50)]
        [ODataRoute("customerAddresses")]
        public IQueryable<CustomerAddress> GetCustomerAddresses()
        {
            return _context.CustomerAddresses;
        }

        // GET: api/customerAddresses(29525,1049)
        [HttpGet]
        [EnableQuery]
        [ODataRoute("customerAddresses({customerId},{addressId})")]
        public IActionResult GetCustomerAddresses([FromODataUri] int customerId, [FromODataUri] int addressId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customerAddress = SingleResult.Create(_context.CustomerAddresses.Where(p => p.CustomerId == customerId && p.AddressId == addressId));

            return Ok(customerAddress);
        }

        // GET: api/products
        [HttpGet]
        [EnableQuery(PageSize = 50)]
        [ODataRoute("products")]
        public IQueryable<Product> GetProducts()
        {
            return _context.Products;
        }

        // GET: api/products(680)
        [HttpGet]
        [EnableQuery]
        [ODataRoute("products({id})")]
        public IActionResult GetProducts([FromODataUri] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = SingleResult.Create(_context.Products.Where(p => p.ProductId == id));

            return Ok(product);
        }

        // GET: api/productCategories
        [HttpGet]
        [EnableQuery(PageSize = 50)]
        [ODataRoute("productCategories")]
        public IQueryable<ProductCategory> GetProductCategories()
        {
            return _context.ProductCategories;
        }

        // GET: api/productCategories(5)
        [HttpGet]
        [EnableQuery]
        [ODataRoute("productCategories({id})")]
        public IActionResult GetProductCategories([FromODataUri] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productCategory = SingleResult.Create(_context.ProductCategories.Where(p => p.ProductCategoryId == id));

            return Ok(productCategory);
        }

        // GET: api/productDescriptions
        [HttpGet]
        [EnableQuery(PageSize = 50)]
        [ODataRoute("productDescriptions")]
        public IQueryable<ProductDescription> GetProductDescriptions()
        {
            return _context.ProductDescriptions;
        }

        // GET: api/productDescriptions(5)
        [HttpGet]
        [EnableQuery]
        [ODataRoute("productDescriptions({id})")]
        public IActionResult GetProductDescriptions([FromODataUri] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productDescription = SingleResult.Create(_context.ProductDescriptions.Where(p => p.ProductDescriptionId == id));

            return Ok(productDescription);
        }

        // GET: api/productModels
        [HttpGet]
        [EnableQuery(PageSize = 50)]
        [ODataRoute("productModels")]
        public IQueryable<ProductModel> GetProductModels()
        {
            return _context.ProductModels;
        }

        // GET: api/productModels(5)
        [HttpGet]
        [EnableQuery]
        [ODataRoute("productModels({id})")]
        public IActionResult GetProductModels([FromODataUri] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productModel = SingleResult.Create(_context.ProductModels.Where(p => p.ProductModelId == id));

            return Ok(productModel);
        }

        // GET: api/productModelProductDescriptions
        [HttpGet]
        [EnableQuery(PageSize = 50)]
        [ODataRoute("productModelProductDescriptions")]
        public IQueryable<ProductModelProductDescription> GetProductModelProductDescriptions()
        {
            return _context.ProductModelProductDescriptions;
        }

        // GET: api/productModelProductDescriptions(2,1210)
        [HttpGet]
        [EnableQuery]
        [ODataRoute("productModelProductDescriptions({productModelId},{productDescriptionId})")]
        public IActionResult GetProductModelProductDescriptions([FromODataUri] int productModelId, [FromODataUri] int productDescriptionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productModelProductDescription = SingleResult.Create(_context.ProductModelProductDescriptions.Where(p => p.ProductModelId == productModelId && p.ProductDescriptionId == productDescriptionId));

            return Ok(productModelProductDescription);
        }

        // GET: api/salesOrderDetails
        [HttpGet]
        [EnableQuery(PageSize = 50)]
        [ODataRoute("salesOrderDetails")]
        public IQueryable<SalesOrderDetail> GetSalesOrderDetails()
        {
            return _context.SalesOrderDetails;
        }

        // GET: api/salesOrderDetails(110562)
        [HttpGet]
        [EnableQuery]
        [ODataRoute("salesOrderDetails({id})")]
        public IActionResult GetSalesOrderDetails([FromODataUri] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var salesOrderDetail = SingleResult.Create(_context.SalesOrderDetails.Where(p => p.SalesOrderDetailId == id));

            return Ok(salesOrderDetail);
        }

        // GET: api/salesOrderHeaders
        [HttpGet]
        [EnableQuery(PageSize = 50)]
        [ODataRoute("salesOrderHeaders")]
        public IQueryable<SalesOrderHeader> GetSalesOrderHeaders()
        {
            return _context.SalesOrderHeaders;
        }

        // GET: api/salesOrderHeaders(71774)
        [HttpGet]
        [EnableQuery]
        [ODataRoute("salesOrderHeaders({id})")]
        public IActionResult GetSalesOrderHeaders([FromODataUri] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var salesOrderHeader = SingleResult.Create(_context.SalesOrderHeaders.Where(p => p.SalesOrderId == id));

            return Ok(salesOrderHeader);
        }
    }
}
