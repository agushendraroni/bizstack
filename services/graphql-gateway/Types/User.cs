


namespace GraphQLGateway.Types
{   


    using HotChocolate.Types.Relay;

    [Key("id")]
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }  // untuk warning nanti bisa ditangani
    }


}