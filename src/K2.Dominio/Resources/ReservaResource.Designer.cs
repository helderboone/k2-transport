﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace K2.Dominio.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class ReservaResource {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ReservaResource() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("K2.Dominio.Resources.ReservaResource", typeof(ReservaResource).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A reserva com o ID informado não existe..
        /// </summary>
        public static string Id_Reserva_Nao_Existe {
            get {
                return ResourceManager.GetString("Id_Reserva_Nao_Existe", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Você não é o motorista escalado para a viagem, logo não é possível visualzar as informações da reserva..
        /// </summary>
        public static string Motorista_Sem_Permissao_Obter {
            get {
                return ResourceManager.GetString("Motorista_Sem_Permissao_Obter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Você não é o proprietário do carro escalado para a viagem, logo não é possível visualzar as informações da reserva..
        /// </summary>
        public static string Proprietario_Sem_Permissao_Obter {
            get {
                return ResourceManager.GetString("Proprietario_Sem_Permissao_Obter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A reserva foi alterada com sucesso..
        /// </summary>
        public static string Reserva_Alterada_Com_Sucesso {
            get {
                return ResourceManager.GetString("Reserva_Alterada_Com_Sucesso", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A reseva foi cadastrada com sucesso..
        /// </summary>
        public static string Reserva_Cadastrada_Com_Sucesso {
            get {
                return ResourceManager.GetString("Reserva_Cadastrada_Com_Sucesso", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A reserva com o ID informado foi encontrada com sucesso..
        /// </summary>
        public static string Reserva_Encontrada_Com_Sucesso {
            get {
                return ResourceManager.GetString("Reserva_Encontrada_Com_Sucesso", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A reserva foi excluída com sucesso..
        /// </summary>
        public static string Reserva_Excluida_Com_Sucesso {
            get {
                return ResourceManager.GetString("Reserva_Excluida_Com_Sucesso", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Já existe uma reserva cadastrada para esse cliente na viagem informada..
        /// </summary>
        public static string Reserva_Ja_Existe_Para_Cliente_Viagem {
            get {
                return ResourceManager.GetString("Reserva_Ja_Existe_Para_Cliente_Viagem", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to As reservas da viagem com o ID informado foram encontradas com sucesso..
        /// </summary>
        public static string Reservas_Viagem_Encontradas_Com_Sucesso {
            get {
                return ResourceManager.GetString("Reservas_Viagem_Encontradas_Com_Sucesso", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A sequência de embarque é obrigatória e não foi informada..
        /// </summary>
        public static string Sequencia_Embarque_Obrigatoria_Nao_Informado {
            get {
                return ResourceManager.GetString("Sequencia_Embarque_Obrigatoria_Nao_Informado", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to O valor pago da reserva é inválido. Informe um valor maior que zero..
        /// </summary>
        public static string Valor_Pago_Invalido {
            get {
                return ResourceManager.GetString("Valor_Pago_Invalido", resourceCulture);
            }
        }
    }
}
