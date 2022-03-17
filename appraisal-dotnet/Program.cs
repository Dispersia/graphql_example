namespace AppraisalServer;

public class Server
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder
          .Services
          .AddGraphQLServer()
          .AddApolloFederation()
          .AddQueryType<Query>();

        var app = builder.Build();

        app.Urls.Add("http://0.0.0.0:5000");

        app.MapGraphQL();

        app.Run();
    }


    public enum TradeIntent { Sell, NotSure }
  
    public class Appraisal
    {
      [Key]
      public int Id { get; set; }

      [ReferenceResolver]
      public static async Task<Appraisal?> GetByIdAsync([ID] int id)
      {
        var appraisals = new Dictionary<int, Appraisal>
        {
          { 0, new Appraisal { Id = 0 } },
          { 1, new Appraisal { Id = 1 }  }
        };

        return await Task.FromResult(appraisals.TryGetValue(id, out var appraisal) ? appraisal : null);
      }
    }

    public class Query
    {
      public async Task<Appraisal?> GetAppraisal([ID] int id) => await Appraisal.GetByIdAsync(id);
    }
}
