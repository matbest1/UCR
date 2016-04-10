using Newtonsoft.Json.Linq;
using System;
using UCS.Core;
using UCS.GameFiles;
using UCS.Helpers;

namespace UCS.Logic
{
    internal class Obstacle : GameObject
    {
        private readonly Level m_vLevel;
        private Timer m_vTimer;

        public Obstacle(Data data, Level l) : base(data, l)
        {
            m_vLevel = l;
        }

        public override int ClassId
        {
            get { return 3; }
        }

        public void CancelClearing()
        {
            m_vLevel.WorkerManager.DeallocateWorker(this);
            m_vTimer = null;
            var od = GetObstacleData();
            var rd = od.GetClearingResource();
            var cost = od.ClearCost;
            GetLevel().GetPlayerAvatar().CommodityCountChangeHelper(0, rd, cost);
        }

        public void ClearingFinished()
        {
            //gérer achievement
            //gérer obstacleclearcounter
            m_vLevel.GameObjectManager.GetObstacleManager().IncreaseObstacleClearCount();

            //gérer diamond reward
            m_vLevel.WorkerManager.DeallocateWorker(this);
            m_vTimer = null;

            //Add exp to client avatar
            var constructionTime = GetObstacleData().ClearTimeSeconds;
            var exp = (int)Math.Pow(constructionTime, 0.5f);
            GetLevel().GetPlayerAvatar().AddExperience(exp);

            var rd = ObjectManager.DataTables.GetResourceByName(GetObstacleData().LootResource);

            GetLevel().GetPlayerAvatar().CommodityCountChangeHelper(0, rd, GetObstacleData().LootCount);

            GetLevel().GameObjectManager.RemoveGameObject(this);
        }

        public ObstacleData GetObstacleData()
        {
            return (ObstacleData)GetData();
        }

        public int GetRemainingClearingTime()
        {
            return m_vTimer.GetRemainingSeconds(m_vLevel.GetTime());
        }

        public bool IsClearingOnGoing()
        {
            return m_vTimer != null;
        }

        public void SpeedUpClearing()
        {
            var remainingSeconds = 0;
            if (IsClearingOnGoing())
                remainingSeconds = m_vTimer.GetRemainingSeconds(m_vLevel.GetTime());
            var cost = GamePlayUtil.GetSpeedUpCost(remainingSeconds);
            var ca = GetLevel().GetPlayerAvatar();
            if (ca.HasEnoughDiamonds(cost))
            {
                ca.UseDiamonds(cost);
                ClearingFinished();
            }
        }

        public void StartClearing()
        {
            var constructionTime = GetObstacleData().ClearTimeSeconds;
            if (constructionTime < 1)
                ClearingFinished();
            else
            {
                m_vTimer = new Timer();
                m_vTimer.StartTimer(constructionTime, m_vLevel.GetTime());
                m_vLevel.WorkerManager.AllocateWorker(this);
            }
        }

        public override void Tick()
        {
            if (IsClearingOnGoing())
            {
                if (m_vTimer.GetRemainingSeconds(m_vLevel.GetTime()) <= 0)
                    ClearingFinished();
            }
        }

        public JObject ToJson()
        {
            var jsonObject = new JObject();
            jsonObject.Add("data", GetObstacleData().GetGlobalID());
            //const_t à vérifier pour un obstacle
            if (IsClearingOnGoing())
                jsonObject.Add("const_t", m_vTimer.GetRemainingSeconds(m_vLevel.GetTime()));
            jsonObject.Add("x", X);
            jsonObject.Add("y", Y);
            return jsonObject;
        }
    }
}