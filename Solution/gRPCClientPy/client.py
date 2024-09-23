import seminar_pb2_grpc
import seminar_pb2

import grpc

def run():
    with grpc.insecure_channel('localhost:5274') as channel:
        stub = seminar_pb2_grpc.SeminarStub(channel)
        print("Escolha uma das opções abaixo:")
        print("1 - Operação matemática:")
        print("2 - Alterar string:")
        print("3 - Alterar arquivo texto:")
        rpccall = input()

        if rpccall == "1":
            numbera = int(input("Informe o primeiro número: "))
            numberb = int(input("Informe o segundo número: "))
            operation = int(input("Informe a operação (+, -, *, /): "))
            numberOperands = seminar_pb2.NumberOperands(first_op=numbera, second_op=numberb, op_type=operation)
            operationResult = stub.Calculate(numberOperands)
            print(f"Resultado: {operationResult.result}")
        
        elif rpccall == "2":
            text = input("Informe a string: ")
            stringMessage = seminar_pb2.StringMessage(input=text)
            stringResult = stub.TransformString(stringMessage)
            print(f"String alterada: {stringResult.output}")
        
        elif rpccall == "3":
            text = input("Informe o conteúdo do arquivo: ")
            fileOperationRequest = seminar_pb2.FileOperationRequest(content=text)
            fileOperationResult = stub.ModifyFile(fileOperationRequest)
            #print(f"Resultado aquivo: {fileOperationResult.result}")
        
        else:
            print("Opção inválida.")

if __name__ == "__main__":
    run()
