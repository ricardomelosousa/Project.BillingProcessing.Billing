// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: charge.proto
// </auto-generated>
#pragma warning disable 0414, 1591
#region Designer generated code

using grpc = global::Grpc.Core;

namespace GrpcChargeApi {
  public static partial class ChargeProtoService
  {
    static readonly string __ServiceName = "chargeProtoService.V1.ChargeProtoService";

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static void __Helper_SerializeMessage(global::Google.Protobuf.IMessage message, grpc::SerializationContext context)
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (message is global::Google.Protobuf.IBufferMessage)
      {
        context.SetPayloadLength(message.CalculateSize());
        global::Google.Protobuf.MessageExtensions.WriteTo(message, context.GetBufferWriter());
        context.Complete();
        return;
      }
      #endif
      context.Complete(global::Google.Protobuf.MessageExtensions.ToByteArray(message));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static class __Helper_MessageCache<T>
    {
      public static readonly bool IsBufferMessage = global::System.Reflection.IntrospectionExtensions.GetTypeInfo(typeof(global::Google.Protobuf.IBufferMessage)).IsAssignableFrom(typeof(T));
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static T __Helper_DeserializeMessage<T>(grpc::DeserializationContext context, global::Google.Protobuf.MessageParser<T> parser) where T : global::Google.Protobuf.IMessage<T>
    {
      #if !GRPC_DISABLE_PROTOBUF_BUFFER_SERIALIZATION
      if (__Helper_MessageCache<T>.IsBufferMessage)
      {
        return parser.ParseFrom(context.PayloadAsReadOnlySequence());
      }
      #endif
      return parser.ParseFrom(context.PayloadAsNewBuffer());
    }

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GrpcChargeApi.GetChargeByParameterRequest> __Marshaller_chargeProtoService_V1_GetChargeByParameterRequest = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GrpcChargeApi.GetChargeByParameterRequest.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GrpcChargeApi.ChargeMode> __Marshaller_chargeProtoService_V1_ChargeMode = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GrpcChargeApi.ChargeMode.Parser));
    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Marshaller<global::GrpcChargeApi.CreateChargeResponse> __Marshaller_chargeProtoService_V1_CreateChargeResponse = grpc::Marshallers.Create(__Helper_SerializeMessage, context => __Helper_DeserializeMessage(context, global::GrpcChargeApi.CreateChargeResponse.Parser));

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::GrpcChargeApi.GetChargeByParameterRequest, global::GrpcChargeApi.ChargeMode> __Method_GetChargeByParameter = new grpc::Method<global::GrpcChargeApi.GetChargeByParameterRequest, global::GrpcChargeApi.ChargeMode>(
        grpc::MethodType.Unary,
        __ServiceName,
        "GetChargeByParameter",
        __Marshaller_chargeProtoService_V1_GetChargeByParameterRequest,
        __Marshaller_chargeProtoService_V1_ChargeMode);

    [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
    static readonly grpc::Method<global::GrpcChargeApi.ChargeMode, global::GrpcChargeApi.CreateChargeResponse> __Method_CreateCharge = new grpc::Method<global::GrpcChargeApi.ChargeMode, global::GrpcChargeApi.CreateChargeResponse>(
        grpc::MethodType.Unary,
        __ServiceName,
        "CreateCharge",
        __Marshaller_chargeProtoService_V1_ChargeMode,
        __Marshaller_chargeProtoService_V1_CreateChargeResponse);

    /// <summary>Service descriptor</summary>
    public static global::Google.Protobuf.Reflection.ServiceDescriptor Descriptor
    {
      get { return global::GrpcChargeApi.ChargeReflection.Descriptor.Services[0]; }
    }

    /// <summary>Client for ChargeProtoService</summary>
    public partial class ChargeProtoServiceClient : grpc::ClientBase<ChargeProtoServiceClient>
    {
      /// <summary>Creates a new client for ChargeProtoService</summary>
      /// <param name="channel">The channel to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public ChargeProtoServiceClient(grpc::ChannelBase channel) : base(channel)
      {
      }
      /// <summary>Creates a new client for ChargeProtoService that uses a custom <c>CallInvoker</c>.</summary>
      /// <param name="callInvoker">The callInvoker to use to make remote calls.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public ChargeProtoServiceClient(grpc::CallInvoker callInvoker) : base(callInvoker)
      {
      }
      /// <summary>Protected parameterless constructor to allow creation of test doubles.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected ChargeProtoServiceClient() : base()
      {
      }
      /// <summary>Protected constructor to allow creation of configured clients.</summary>
      /// <param name="configuration">The client configuration.</param>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected ChargeProtoServiceClient(ClientBaseConfiguration configuration) : base(configuration)
      {
      }

      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::GrpcChargeApi.ChargeMode GetChargeByParameter(global::GrpcChargeApi.GetChargeByParameterRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetChargeByParameter(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::GrpcChargeApi.ChargeMode GetChargeByParameter(global::GrpcChargeApi.GetChargeByParameterRequest request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_GetChargeByParameter, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::GrpcChargeApi.ChargeMode> GetChargeByParameterAsync(global::GrpcChargeApi.GetChargeByParameterRequest request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return GetChargeByParameterAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::GrpcChargeApi.ChargeMode> GetChargeByParameterAsync(global::GrpcChargeApi.GetChargeByParameterRequest request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_GetChargeByParameter, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::GrpcChargeApi.CreateChargeResponse CreateCharge(global::GrpcChargeApi.ChargeMode request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateCharge(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual global::GrpcChargeApi.CreateChargeResponse CreateCharge(global::GrpcChargeApi.ChargeMode request, grpc::CallOptions options)
      {
        return CallInvoker.BlockingUnaryCall(__Method_CreateCharge, null, options, request);
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::GrpcChargeApi.CreateChargeResponse> CreateChargeAsync(global::GrpcChargeApi.ChargeMode request, grpc::Metadata headers = null, global::System.DateTime? deadline = null, global::System.Threading.CancellationToken cancellationToken = default(global::System.Threading.CancellationToken))
      {
        return CreateChargeAsync(request, new grpc::CallOptions(headers, deadline, cancellationToken));
      }
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      public virtual grpc::AsyncUnaryCall<global::GrpcChargeApi.CreateChargeResponse> CreateChargeAsync(global::GrpcChargeApi.ChargeMode request, grpc::CallOptions options)
      {
        return CallInvoker.AsyncUnaryCall(__Method_CreateCharge, null, options, request);
      }
      /// <summary>Creates a new instance of client from given <c>ClientBaseConfiguration</c>.</summary>
      [global::System.CodeDom.Compiler.GeneratedCode("grpc_csharp_plugin", null)]
      protected override ChargeProtoServiceClient NewInstance(ClientBaseConfiguration configuration)
      {
        return new ChargeProtoServiceClient(configuration);
      }
    }

  }
}
#endregion
