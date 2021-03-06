﻿using RestSharp;
using System.Collections.Generic;
using System.Linq;

namespace XolphinApiDotNet.Endpoint
{
    public class Support
    {
        private Client client;

        public Support(Client client)
        {
            this.client = client;
        }

        public List<string> ApproverEmailAddresses(string domain)
        {
            return client.Get<List<string>>("approver-email-addresses", "domain", domain, ParameterType.QueryString);
        }

        public Responses.CSR DecodeCSR(string csr)
        {
            return client.PostSingle<Responses.CSR>("decode-csr", "csr", csr);
        }

        public List<Responses.Product> Products()
        {
            IEnumerable<Responses.Product> products = new List<Responses.Product>();

            var result = client.Get<Responses.AllProducts>("products", "page", 1, ParameterType.QueryString);

            if (!result.isError())
            {
                products = result.Products;
                while (result.Page < result.Pages)
                {
                    result = client.Get<Responses.AllProducts>("products", "page", result.Page + 1, ParameterType.QueryString);
                    if (result.isError()) break;
                    products = products.Union(result.Products);
                }
            }

            return products.ToList();
        }

        public Responses.Product Product(int id)
        {
            return client.Get<Responses.Product>("products/{id}", "id", id, ParameterType.UrlSegment);
        }
    }
}