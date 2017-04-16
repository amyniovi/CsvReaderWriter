using AddressProcessing.Address;
using AddressProcessing.Address.v1;
using AddressProcessing.CSV;
using NUnit.Framework;

namespace AddressProcessing.Tests
{
    [TestFixture]
    public class AddressFileProcessorTests
    {
        private FakeMailShotService _fakeMailShotService;
        //we want the test to still pass as an integration test with the test data...hence using the actual CSVStreamProvider
        private FileStreamProvider _fileStreamProvider;
        private const string TestInputFile = @"test_data\contacts.csv";

        [SetUp]
        public void SetUp()
        {
            _fakeMailShotService = new FakeMailShotService();
            _fileStreamProvider = new FileStreamProvider();
        }

        [Test]
        public void Should_send_mail_using_mailshot_service()
        {
            var processor = new AddressFileProcessor(_fakeMailShotService, _fileStreamProvider);
            processor.Process(TestInputFile);

            Assert.That(_fakeMailShotService.Counter, Is.EqualTo(229));
        }

        internal class FakeMailShotService : IMailShot
        {
            internal int Counter { get; private set; }

            public void SendMailShot(string name, string address)
            {
                Counter++;
            }
        }
    }
}