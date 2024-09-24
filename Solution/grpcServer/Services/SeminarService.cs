using Grpc.Core;
using grpcServer;
using System.Threading.Tasks;

namespace grpcServer.Services;

public class SeminarService : Seminar.SeminarBase{
    private readonly ILogger<SeminarService> _logger;
    public SeminarService(ILogger<SeminarService> logger){
        _logger = logger;
    }

    public override Task<OperationResult> Calculate(NumberOperands request, ServerCallContext context){
        float result = 0.0;
        switch(request.OpType){
            case 1:
                result = request.FirstOp + request.SecondOp;
                break;
            case 2:
                result = request.FirstOp - request.SecondOp;
                break;
            case 3:
                result = request.FirstOp * request.SecondOp;
                break;
            case 4:
                if(request.SecondOp != 0){
                    result = request.FirstOp / request.SecondOp;
                }else{
                    Console.WriteLine("Não é possível dividir por 0!");
                }
                break;
            default:
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid operation type"));
        }

        return Task.FromResult(new OperationResult{
            Result = result
        });
    }

    public override Task<StringResult> TransformString(StringMessage request, ServerCallContext context){
        return Task.FromResult(new StringResult{
            Output = request.Input.ToUpper()
        });
    }


    public override Task<FileOperationResult> ModifyFile(FileOperationRequest request, ServerCallContext context){
        bool success;
        string content = request.Content;
        string relativePath = "../testfile.txt";
        string absolutePath = Path.GetFullPath(relativePath);

        try{
            File.WriteAllText(absolutePath, content);
            success = true;
        }catch(System.Exception){
            success = false;
        } 
        return Task.FromResult(new FileOperationResult{
            Success = success
        });
    }
}
