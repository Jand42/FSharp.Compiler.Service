// Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

module internal Microsoft.FSharp.Compiler.Driver 

open Microsoft.FSharp.Compiler.Ast
open Microsoft.FSharp.Compiler.AbstractIL.IL
open Microsoft.FSharp.Compiler.AbstractIL
open Microsoft.FSharp.Compiler.ErrorLogger
open Microsoft.FSharp.Compiler.CompileOps
open Microsoft.FSharp.Compiler.TcGlobals
open Microsoft.FSharp.Compiler.Tast
open Microsoft.FSharp.Compiler.TypeChecker

[<AbstractClass>]
type ErrorLoggerProvider =
    new : unit -> ErrorLoggerProvider
    abstract CreateErrorLoggerUpToMaxErrors : tcConfigBuilder : TcConfigBuilder * exiter : Exiter -> ErrorLogger

type StrongNameSigningInfo 

val EncodeInterfaceData: tcConfig:TcConfig * tcGlobals:TcGlobals * exportRemapping:Tastops.Remap * generatedCcu: Tast.CcuThunk * outfile: string * isIncrementalBuild: bool -> ILAttribute list * ILResource list
val ValidateKeySigningAttributes : tcConfig:TcConfig * tcGlobals:TcGlobals * TypeChecker.TopAttribs -> StrongNameSigningInfo
val GetStrongNameSigner : StrongNameSigningInfo -> ILBinaryWriter.ILStrongNameSigner option

/// Proccess the given set of command line arguments
val internal ProcessCommandLineFlags : TcConfigBuilder * setProcessThreadLocals:(TcConfigBuilder -> unit) * lcidFromCodePage : int option * argv:string[] -> string list

//---------------------------------------------------------------------------
// The entry point used by fsc.exe

val typecheckAndCompile : 
    argv : string[] * 
    referenceResolver: ReferenceResolver.Resolver * 
    bannerAlreadyPrinted : bool * 
    openBinariesInMemory: bool * 
    exiter : Exiter *
    loggerProvider: ErrorLoggerProvider *
    tcImportsCapture: (TcImports -> unit) option *
    dynamicAssemblyCreator: (TcGlobals * string * ILModuleDef -> unit) option
      -> unit

val mainCompile : 
    argv: string[] * 
    referenceResolver: ReferenceResolver.Resolver * 
    bannerAlreadyPrinted: bool * 
    openBinariesInMemory: bool * 
    exiter: Exiter * 
    loggerProvider: ErrorLoggerProvider * 
    tcImportsCapture: (TcImports -> unit) option *
    dynamicAssemblyCreator: (TcGlobals * string * ILModuleDef -> unit) option
      -> unit

val compileOfAst : 
    referenceResolver: ReferenceResolver.Resolver * 
    openBinariesInMemory: bool * 
    assemblyName:string * 
    target:CompilerTarget * 
    targetDll:string * 
    targetPdb:string option * 
    dependencies:string list * 
    noframework:bool *
    exiter:Exiter * 
    loggerProvider: ErrorLoggerProvider * 
    inputs:ParsedInput list *
    tcImportsCapture : (TcImports -> unit) option *
    dynamicAssemblyCreator: (TcGlobals * string * ILModuleDef -> unit) option
      -> unit

val compileOfAst : 
    referenceResolver: ReferenceResolver.Resolver * 
    openBinariesInMemory: bool * 
    assemblyName:string * 
    target:CompilerTarget * 
    targetDll:string * 
    targetPdb:string option * 
    dependencies:string list * 
    noframework:bool *
    exiter:Exiter * 
    loggerProvider: ErrorLoggerProvider * 
    inputs:ParsedInput list *
    tcImportsCapture : (TcImports -> unit) option *
    dynamicAssemblyCreator: (TcGlobals * string * ILModuleDef -> unit) option
      -> unit

val compileChecked :
    tcGlobals : TcGlobals *
    tcImports : TcImports *
    frameworkTcImports : TcImports *
    generatedCcu : CcuThunk *
    typedImplFiles : TypedImplFile list *
    topAttrs : TopAttribs *
    tcConfig : TcConfig * 
    outfile : string *
    pdbFile : string option *
    assemblyName : string *
    errorLogger : ErrorLogger *
    exiter : Exiter * 
    tcImportsCapture : (TcImports -> unit) option *
    dynamicAssemblyCreator: (TcGlobals * string * ILModuleDef -> unit) option
      -> unit

/// Part of LegacyHostedCompilerForTesting
type InProcErrorLoggerProvider = 
    new : unit -> InProcErrorLoggerProvider
    member Provider : ErrorLoggerProvider
    member CapturedWarnings : Diagnostic[]
    member CapturedErrors : Diagnostic[]


module internal MainModuleBuilder =
    
    val fileVersion: warn: (exn -> unit) -> findStringAttr: (string -> string option) -> assemblyVersion: AbstractIL.IL.ILVersionInfo -> AbstractIL.IL.ILVersionInfo
    val productVersion: warn: (exn -> unit) -> findStringAttr: (string -> string option) -> fileVersion: AbstractIL.IL.ILVersionInfo -> string
    val productVersionToILVersionInfo: string -> AbstractIL.IL.ILVersionInfo
