using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.GameComponents;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace ModTest
{
    public class CompanionLimitTweak : DefaultClanTierModel
    {
        public override int GetCompanionLimit(TaleWorlds.CampaignSystem.Clan clan)
        {
            if (clan.Leader == Hero.MainHero)
                return base.GetCompanionLimit(clan) * 50;
            return base.GetCompanionLimit(clan);
        }
        public override int GetPartyLimitForTier(Clan clan, int clanTierToCheck)
        {
            if (clan.Leader == Hero.MainHero)
                return base.GetPartyLimitForTier(clan, clanTierToCheck) * 10;
            return base.GetPartyLimitForTier(clan, clanTierToCheck);
        }
    }
    public class IncreaseParty : DefaultPartySizeLimitModel
    {
        public override ExplainedNumber GetPartyMemberSizeLimit(PartyBase party, bool includeDescriptions = false)
        {
            if (party.LeaderHero == Hero.MainHero)
                return new ExplainedNumber(base.GetPartyMemberSizeLimit(party, includeDescriptions).BaseNumber * 10, false);
            return base.GetPartyMemberSizeLimit(party, includeDescriptions);
        }
        public override ExplainedNumber GetPartyPrisonerSizeLimit(PartyBase party, bool includeDescriptions = false)
        {
            if (party.LeaderHero == Hero.MainHero)
                return new ExplainedNumber(base.GetPartyPrisonerSizeLimit(party, includeDescriptions).BaseNumber * 10, false);
            return base.GetPartyPrisonerSizeLimit(party, includeDescriptions);
        }
    }

    public class RenownTweak : DefaultBattleRewardModel
    {
        public override ExplainedNumber CalculateRenownGain(PartyBase party, float renownValueOfBattle, float contributionShare)
        {
            if (party.LeaderHero == Hero.MainHero)
            {
                party.LeaderHero.ChangeHeroGold(9999999);
                return base.CalculateRenownGain(party, renownValueOfBattle * 10, contributionShare);
            }
            return base.CalculateRenownGain(party, renownValueOfBattle * 1, contributionShare);
        }
        public override ExplainedNumber CalculateInfluenceGain(PartyBase party, float influenceValueOfBattle, float contributionShare)
        {
            if (party.LeaderHero == Hero.MainHero)
                return base.CalculateInfluenceGain(party, influenceValueOfBattle * 5, contributionShare);
            return base.CalculateInfluenceGain(party, influenceValueOfBattle, contributionShare);
        }
    }

    public class Food10TimesLower : DefaultMobilePartyFoodConsumptionModel
    {
        public override ExplainedNumber CalculateDailyFoodConsumptionf(MobileParty party, ExplainedNumber baseConsumption)
        {
            if (party.LeaderHero == Hero.MainHero)
                return new ExplainedNumber(base.CalculateDailyFoodConsumptionf(party, baseConsumption).BaseNumber / 10f, false);
            return new ExplainedNumber(base.CalculateDailyFoodConsumptionf(party, baseConsumption).BaseNumber, false);
        }
    }

    public class NoEnergyLossFromSmithing : DefaultSmithingModel
    {
        public override int GetEnergyCostForRefining(ref Crafting.RefiningFormula refineFormula, Hero hero)
        {
            return 0;
        }
        public override int GetEnergyCostForSmelting(ItemObject item, Hero hero)
        {
            return 0;
        }
        public override int GetEnergyCostForSmithing(ItemObject item, Hero hero)
        {
            return 0;
        }
    }

    public class ReduceWages : DefaultPartyWageModel
    {
        public override ExplainedNumber GetTotalWage(MobileParty mobileParty, bool includeDescriptions = false)
        {
            if (mobileParty.LeaderHero == Hero.MainHero)
                return new ExplainedNumber(base.GetTotalWage(mobileParty, includeDescriptions).BaseNumber / 20f, false);
            return base.GetTotalWage(mobileParty, includeDescriptions);
        }
    }
    public class IncreaseCarry : DefaultInventoryCapacityModel
    {
        public override ExplainedNumber CalculateInventoryCapacity(MobileParty mobileParty, bool includeDescriptions = false, int additionalTroops = 0, int additionalSpareMounts = 0, int additionalPackAnimals = 0, bool includeFollowers = false)
        {
            if (mobileParty.LeaderHero == Hero.MainHero)
                return new ExplainedNumber(base.CalculateInventoryCapacity(mobileParty, includeDescriptions, additionalTroops, additionalSpareMounts, additionalPackAnimals, includeFollowers).BaseNumber + 10_000f, false);
            return base.CalculateInventoryCapacity(mobileParty, includeDescriptions, additionalTroops, additionalSpareMounts, additionalPackAnimals, includeFollowers);
        }
    }

    public class MoveSpeed : DefaultPartySpeedCalculatingModel
    {
        public override ExplainedNumber CalculateFinalSpeed(MobileParty mobileParty, ExplainedNumber finalSpeed)
        {
            if (mobileParty.LeaderHero == Hero.MainHero)
                return new ExplainedNumber(base.CalculateFinalSpeed(mobileParty, finalSpeed).BaseNumber + 6f, false);
            return base.CalculateFinalSpeed(mobileParty, finalSpeed);
        }
    }
    public class Main : MBSubModuleBase
    {
        protected override void OnGameStart(Game game, IGameStarter starterObject)
        {
            base.OnGameStart(game, starterObject);
            starterObject.AddModel(new RenownTweak());
            starterObject.AddModel(new CompanionLimitTweak());
            starterObject.AddModel(new Food10TimesLower());
            starterObject.AddModel(new NoEnergyLossFromSmithing());
            starterObject.AddModel(new MoveSpeed());
            starterObject.AddModel(new IncreaseParty());
            starterObject.AddModel(new ReduceWages());
            starterObject.AddModel(new IncreaseCarry());
        }


    }
}
