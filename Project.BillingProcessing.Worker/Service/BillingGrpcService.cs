

using AutoMapper;
using GrpcChargeApi;
using GrpcCustomers;

using Project.BillingProcessing.Worker.Model;

namespace Project.BillingProcessing.Charge.Api.Service
{
    public class BillingGrpcService
    {
        private readonly CustomerProtoService.CustomerProtoServiceClient _customerProtoServiceClient;
        private readonly ChargeProtoService.ChargeProtoServiceClient _chargeProtoServiceClient;
        private readonly IMapper _mapper;

        public BillingGrpcService(CustomerProtoService.CustomerProtoServiceClient customerProtoServiceClient, ChargeProtoService.ChargeProtoServiceClient chargeProtoServiceClient, IMapper mapper)
        {
            _customerProtoServiceClient = customerProtoServiceClient;
            _chargeProtoServiceClient = chargeProtoServiceClient;
            _mapper = mapper;
        }

        public async Task<CustomerModel> GetCustomerValid(string identification)
        {
            var request = new GetCustomerByIdentificationRequest { Identification = identification };
            var response = await _customerProtoServiceClient.GetCustomerByIdentificationAsync(request);
            return _mapper.Map<CustomerModel>(response);
        }
        public async Task<bool> CreateCharge(ChargeWorkerModel chargeModel)
        {
            var result = _chargeProtoServiceClient.CreateCharge(_mapper.Map<GrpcChargeApi.ChargeModel>(chargeModel));
            return await Task.FromResult(result.Success);
        }

        public async Task<List<CustomerModel>> GetAllCustomers(int take)
        {
            var body = new CustomerTake() { Take = take };
            var response = _customerProtoServiceClient.GetAll(body);

            return await Task.FromResult(_mapper.Map<List<CustomerModel>>(response));

        }
    }
}
