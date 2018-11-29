﻿using System;
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
        private int discount;

        public Product(string name, int cost, int count, float weight)
        {
            this.name = name;
            this.cost = cost;
            this.count = count;
            this.totalCost = cost * count;
            this.weight = weight;
            this.discount = 0;
        }

        public Product(string name, int cost, int count, float weight, int discount)
        {
            this.name = name;
            this.cost = cost;
            this.count = count;
            this.totalCost = cost * count;
            this.weight = weight;
            this.discount = discount;
        }

        public ProductBind bindData()
        {

            ProductBind data = new ProductBind() { name = this.name, cost = this.cost, count = this.count, totalCost = this.totalCost, weight = this.weight, discount = this.discount };

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

        public int getDiscount()
        {
            return discount;
        }

        public void setCount(int count)
        {
            this.count = count;
        }

        public void setTotalCost(int totalCost)
        {
            this.totalCost = totalCost;
        }

        public void setDiscount(int discount)
        {
            this.discount = discount;
        }
    }

    class ProductBind
    {
        public string name { get; set; }
        public int cost { get; set; }
        public int count { get; set; }
        public int totalCost { get; set; }
        public float weight { get; set; }
        public int discount { get; set; }
    }

    class TotalInfo
    {
        public int totalCost { get; set; }
        public float weight { get; set; }
    }
}
