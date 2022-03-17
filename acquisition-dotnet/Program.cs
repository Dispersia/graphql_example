namespace AcquisitionServer;

public class Server {
  public static void Main(string[] args) {
    var builder = WebApplication.CreateBuilder(args);

    builder.Services
      .AddGraphQLServer()
      .AddApolloFederation()
      .AddQueryType<Query>()
      .AddType<Appraisal>();

    var app = builder.Build();

    app.Urls.Add("http://0.0.0.0:5001");

    app.UseRouting();

    app.UseEndpoints(endpoints =>
      endpoints.MapGraphQL()
    );

    app.Run();
  }
}

public class Acquisition
{
  [Key]
  public int Id { get; set; }

  public SomethingElse Possibly { get; set; }

  [ReferenceResolver]
  public static async Task<Acquisition?> GetAcquisitionById(
    [ID] int id)
  {
    var acquisitions = new Dictionary<int, Acquisition>
    {
      { 0, new Acquisition { Id = 0, Possibly = SomethingElse.Something } },
      { 1, new Acquisition { Id = 1, Possibly = SomethingElse.Else } }
    };

    return await Task.FromResult(acquisitions.TryGetValue(id, out var acquisition) ? acquisition : null);
  }
}

[ExtendServiceType]
public class Appraisal
{
  [Key]
  [External]
  public int Id { get; set; }
  
  public async Task<Acquisition?> GetAcquisition()
    => await Acquisition.GetAcquisitionById(Id);
}

public class Query
{
  public async Task<Acquisition?> GetAcquisition([ID] int id)
    => await Acquisition.GetAcquisitionById(id);
}

public enum SomethingElse { Something, Else }
