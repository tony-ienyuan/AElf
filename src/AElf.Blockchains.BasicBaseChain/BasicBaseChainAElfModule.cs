﻿using System;
using AElf.CrossChain.Grpc;
using AElf.Kernel;
using AElf.Kernel.Consensus.DPoS;
using AElf.Kernel.SmartContract;
using AElf.Kernel.SmartContract.Sdk;
using AElf.Modularity;
using AElf.OS;
using AElf.OS.Network.Grpc;
using AElf.OS.Rpc.ChainController;
using AElf.OS.Rpc.Net;
using AElf.OS.Rpc.Wallet;
using AElf.Runtime.CSharp;
using AElf.RuntimeSetup;
using AElf.WebApp.Web;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.AspNetCore;
using Volo.Abp.Modularity;

namespace AElf.Blockchains.BasicBaseChain
{
    [DependsOn(
        typeof(DPoSConsensusAElfModule),
        typeof(KernelAElfModule),
        typeof(OSAElfModule),
        typeof(AbpAspNetCoreModule),
        typeof(CSharpRuntimeAElfModule),
        typeof(GrpcNetworkModule),

        //TODO: should move to OSAElfModule
        typeof(ChainControllerRpcModule),
        typeof(WalletRpcModule),
        typeof(NetRpcAElfModule),
        typeof(RuntimeSetupAElfModule),
        typeof(GrpcCrossChainAElfModule),

        //web api module
        typeof(WebWebAppAElfModule)
    )]
    public class BasicBaseChainAElfModule : AElfModule<BasicBaseChainAElfModule>
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var config = context.Services.GetConfiguration();
            var chainType = config.GetValue("ChainType", ChainType.MainChain);

            //TODO: change node type to net type
            var nodeType = config.GetValue("NodeType", NodeType.MainNet);

            //TODO: don't write here, should in startup file
            context.Services.AddConfiguration(new ConfigurationBuilderOptions
            {
                EnvironmentName = $"{chainType}.{nodeType}"
            });

            Configure<TokenInitialOptions>(context.Services.GetConfiguration().GetSection("TokenInitial"));
            Configure<ChainOptions>(option =>
            {
                option.ChainId =
                    ChainHelpers.ConvertBase58ToChainId(context.Services.GetConfiguration()["ChainId"]);
            });

            switch (nodeType)
            {
                case NodeType.MainNet:

                    Configure<HostSmartContractBridgeContextOptions>(options =>
                    {
                        options.ContextVariables[ContextVariableDictionary.NativeSymbolName] = "ELF";
                    });

                    break;
                case NodeType.TestNet:
                    Configure<HostSmartContractBridgeContextOptions>(options =>
                    {
                        options.ContextVariables[ContextVariableDictionary.NativeSymbolName] = "ELFT";
                    });

                    break;
            }
        }
    }


    public enum ChainType
    {
        MainChain,
        SideChain
    }

    public enum NodeType
    {
        MainNet,
        TestNet,
        CustomNet
    }
}