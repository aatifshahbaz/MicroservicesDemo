namespace Catalog.Service.Test.Fixtures
{
    //Class Fixtures
    //When to use: when you want to create a single test context and share it among all the tests in the class,
    //and have it cleaned up after all the tests in the class have finished.

    //Sometimes test context creation and cleanup can be very expensive.If you were to run the creation and cleanup code during every test,
    //it might make the tests slower than you want.You can use the class fixture feature of xUnit.net to share a single object instance among
    //all tests in a test class.

    //We already know that xUnit.net creates a new instance of the test class for every test.When using a class fixture, xUnit.net will ensure
    //that the fixture instance will be created before any of the tests have run, and once all the tests have finished, it will clean up the
    //fixture object by calling Dispose, if present.

    //To use class fixtures, you need to take the following steps:
    //- Create the fixture class, and put the startup code in the fixture class constructor.
    //- If the fixture class needs to perform cleanup, implement IDisposable on the fixture class, and put the cleanup code in the Dispose() method.
    //- Add IClassFixture<> to the test class.
    //- If the test class needs access to the fixture instance, add it as a constructor argument, and it will be provided automatically.


    //Fixture to share across many tests in the same context
    public class SharedContextFixture : IDisposable
    {
        public Queue<Guid> Ids { get; set; }

        public SharedContextFixture()
        {
            // Initialize shared resources here
            Ids = new Queue<Guid>();
        }

        public void Dispose()
        {
            Ids.Clear();
        }
    }
}
