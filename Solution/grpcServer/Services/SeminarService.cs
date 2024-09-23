using Grpc.Core;
using grpcServer;
using System.Threading.Tasks;

namespace grpcServer.Services;

public class SeminarService : Seminar.SeminarBase{
    private readonly ILogger<SeminarService> _logger;
    public SeminarService(ILogger<SeminarService> logger){
        _logger = logger;
    }

    /* Caso a opção 1 seja escolhida no cliente, os números e a operação informados, são recebidos
       para que a operação matemática seja executada */
    public override Task<OperationResult> Calculate(NumberOperands request, ServerCallContext context){
        int result = 0;
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
                    result = 0; 
                }
                break;
            default:
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid operation type"));
        }

        return Task.FromResult(new OperationResult{
            Result = result
        });
    }

    /* Caso a opção 2 seja escolhida, a string informada pelo usuário é passada para ser convertida em
        caracteres maiúsculos */
    public override Task<StringResult> TransformString(StringMessage request, ServerCallContext context){
        return Task.FromResult(new StringResult{
            Output = request.Input.ToUpper()
        });
    }

    /* Caso a opção 3 seja escolhida, o conteúdo a ser inserido no arquivo texto é passado */
    public override Task<FileOperationResult> ModifyFile(FileOperationRequest request, ServerCallContext context){
        bool success;
        string content = request.Content; // conteúdo passado pelo cliente

        // Arquivo de destino
        string relativePath = "../testfile.txt";
        string absolutePath = Path.GetFullPath(relativePath);

        try{
            File.WriteAllText(absolutePath, content);
            success = true; // Se a operação for bem sucedida 
        }catch(System.Exception){
            success = false; // Se houver algum erro
        }

        // Resultado final (sucesso ou falha)
        return Task.FromResult(new FileOperationResult{
            Success = success 
        });
    }
}
