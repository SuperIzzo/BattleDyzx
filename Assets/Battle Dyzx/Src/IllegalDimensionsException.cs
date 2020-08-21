namespace BattleDyzx
{
    public class IllegalDimensionsException : System.ArgumentException
    {        
        public IllegalDimensionsException() : base()
        {
        }

        public IllegalDimensionsException( string message ) 
            : base( message )
        {
        }

        public IllegalDimensionsException( string message, string paramName ) 
            : base( message, paramName )
        {
        }
    }
}