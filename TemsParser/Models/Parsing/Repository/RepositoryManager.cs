using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemsParser.Models.Config;
using TemsParser.Models.Parsing.Area;
using TemsParser.Models.Parsing.Point;
using TemsParser.Models.Settings;

namespace TemsParser.Models.Parsing.Repository
{
    public class RepositoryManager
    {
        private readonly IKernel Kernel;


        public RepositoryManager(SettingsModel setting,
                                 IEnumerable<TechnologyListItemModel> techList,
                                 IEnumerable<OperatorListItemModel> operList,
                                 string directoryBase)
        {
            Kernel = new StandardKernel();

            Kernel.Bind<SettingsModel>().ToConstant(setting);

            if (setting.BinningEnabled)
            {
                if (setting.DefineBestFreqEnabled)
                {
                    Kernel.Bind<RepositoryBase>().To<RepositoryBinModel<PointFreqModel>>()
                        .WithConstructorArgument(techList)
                        .WithConstructorArgument(operList)
                        .WithConstructorArgument(directoryBase);

                }
                else
                {
                    Kernel.Bind<RepositoryBase>().To<RepositoryBinModel<PointModel>>()
                        .WithConstructorArgument(techList)
                        .WithConstructorArgument(operList)
                        .WithConstructorArgument(directoryBase);
                }
            }
            else
            {
                if (setting.DefineBestFreqEnabled)
                {
                    Kernel.Bind<RepositoryBase>().To<RepositoryModel>()
                        .WithConstructorArgument(techList)
                        .WithConstructorArgument(operList)
                        .WithConstructorArgument(directoryBase)
                        .WithConstructorArgument(true);
                }
                else
                {
                    Kernel.Bind<RepositoryBase>().To<RepositoryModel>()
                        .WithConstructorArgument(techList)
                        .WithConstructorArgument(operList)
                        .WithConstructorArgument(directoryBase)
                        .WithConstructorArgument(false);
                }
            }
        }


        public RepositoryBase GetRepository()
        {
            return Kernel.Get<RepositoryBase>();
        }
    }
}
