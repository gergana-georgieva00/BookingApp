﻿using BookingApp.Models.Hotels.Contacts;
using BookingApp.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.Repositories
{
    public class HotelRepository : IRepository<IHotel>
    {
        private List<IHotel> hotels;

        public HotelRepository()
        {
            this.hotels = new List<IHotel>();
        }

        public void AddNew(IHotel model)
        {
            this.hotels.Add(model);
        }

        public IReadOnlyCollection<IHotel> All()
        {
            throw new NotImplementedException();
        }

        public IHotel Select(string criteria)
        {
            throw new NotImplementedException();
        }
    }
}
