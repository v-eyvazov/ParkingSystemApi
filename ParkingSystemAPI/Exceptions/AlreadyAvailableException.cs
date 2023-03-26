namespace ParkingSystemAPI.Exceptions
{
    [Serializable]
    public class AlreadyAvailableException : Exception
    {
        public AlreadyAvailableException() : base("Parking Lot already available") { }
    }
}
