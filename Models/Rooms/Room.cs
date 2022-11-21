using BookingApp.Models.Rooms.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookingApp.Models.Rooms
{
    public class Room : IRoom
    {
        private int bedCapacity;
        private double pricePerNight;

        public Room()
        {

        }

        public int BedCapacity 
        {
            get => this.bedCapacity;
            private set
            {

            }
        }

        public double PricePerNight
        {
            get => this.pricePerNight;
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException(Utilities.Messages.ExceptionMessages.PricePerNightNegative);
                }
            }
        }

        public void SetPrice(double price)
        {
            throw new NotImplementedException();
        }
    }
}
