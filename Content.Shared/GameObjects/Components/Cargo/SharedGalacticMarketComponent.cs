using Content.Shared.Prototypes.Cargo;
using Robust.Shared.GameObjects;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.ViewVariables;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Content.Shared.GameObjects.Components.Cargo
{
    public class SharedGalacticMarketComponent : Component, IEnumerable<CargoProductPrototype>
    {
        public override string Name => "GalacticMarket";
        public override uint? NetID => ContentNetIDs.GALACTIC_MARKET;
        public override Type StateType => typeof(GalacticMarketState);

        [ViewVariables]
        protected List<CargoProductPrototype> _products = new List<CargoProductPrototype>();

        public IEnumerator<CargoProductPrototype> GetEnumerator()
        {
            return _products.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        ///     Returns a list with the IDs of all products.
        /// </summary>
        /// <returns>A list of product IDs</returns>
        public List<string> GetProductIdList()
        {
            List<string> productIds = new List<string>();

            foreach (var product in _products)
            {
                productIds.Add(product.ID);
            }

            return productIds;
        }

        public override void ExposeData(ObjectSerializer serializer)
        {
            base.ExposeData(serializer);

            if (serializer.Reading)
            {
                var products = serializer.ReadDataField("cargoProduct", new List<string>());
                var prototypeManager = IoCManager.Resolve<IPrototypeManager>();
                foreach (var id in products)
                {
                    if (!prototypeManager.TryIndex(id, out CargoProductPrototype recipe))
                        continue;
                    _products.Add(recipe);
                }
            }
            else if (serializer.Writing)
            {
                var products = GetProductIdList();
                serializer.DataField(ref products, "cargoProduct", new List<string>());
            }
        }
    }

    [Serializable, NetSerializable]
    public class GalacticMarketState : ComponentState
    {
        public List<string> Products;
        public GalacticMarketState(List<string> technologies) : base(ContentNetIDs.GALACTIC_MARKET)
        {
            Products = technologies;
        }
    }
}
