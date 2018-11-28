using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConvenienceStore
{
    class Product
    {
        private string name;
        private int cost;
        private int count;
        private int totalCost;
        private float weight;

        public Product(string name, int cost, int count, float weight)
        {
            this.name = name;
            this.cost = cost;
            this.count = count;
            this.totalCost = cost * count;
            this.weight = weight;
        }
        
        public ProductBind bindData()
        {
            ProductBind data = new ProductBind() { name = this.name, cost = this.cost, count = this.count, totalCost = this.totalCost };
            return data;
        }

        public int getTotalCost()
        {
            return totalCost;
        }

        public float getWeight()
        {
            return weight;
        }

        public int getCount()
        {
            return count;
        }

        public int getCost()
        {
            return cost;
        }

        public void setCount(int count)
        {
            this.count = count;
        }

        public void setTotalCost(int totalCost)
        {
            this.totalCost = totalCost;
        }
    }

    class ProductBind
    {
        public string name { get; set; }
        public int cost { get; set; }
        public int count { get; set; }
        public int totalCost { get; set; }
    }

    class TotalInfo
    {
        public int totalCost { get; set; }
        public float weight { get; set; }
    }
}
