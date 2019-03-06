using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Class
{
    class UrbanEnvironment : MonoBehaviour
    {
        private float currentLeftStreetLength = 0;
        private float currentRightStreetLength = 0;
        private float distanceBeetwenBildings = 10;
        private float distanceToRoad = 2;
        private float startPozition = 0;
        private float wallWidth = 0;
        private float wallHeight = 0;
        private float districtLength = 50;
        private float currentDistrictLength = 0;
        private District currentDistrict = District.Low;
        private int buildingColor = 0;
        private int downColor = 0;
        private int maxNumberOfColorSchema = 4;
        private Material[] buildingMaterials;
        public float CurrentWallLength
        {
            get
            {
                if (currentLeftStreetLength <= currentRightStreetLength)
                    return currentLeftStreetLength;
                else
                    return currentRightStreetLength;
            }
        }
        public UrbanEnvironment(float startZPosition, float widthOfWall, float heightOfWall)
        {
            startPozition = startZPosition;
            wallWidth = widthOfWall;
            wallHeight = heightOfWall;
            Initialization();
        }
        void Initialization()
        {
            currentDistrict = (District)Random.Range(0, 3);
            buildingColor = Random.Range(0, maxNumberOfColorSchema);
            downColor = Random.Range(0, maxNumberOfColorSchema);
            //Download materials
            buildingMaterials = new Material[maxNumberOfColorSchema];
            for(int i = 1; i <= maxNumberOfColorSchema; i++)
                buildingMaterials[i-1] = Resources.Load("mat_building" + i.ToString(), typeof(Material)) as Material;
        }
        public void Put()
        {
            //Initialization
            GameObject leftBuilding = SelectBuilding(Side.Left);
            Building leftBuildingData = leftBuilding.GetComponent<Building>();
            GameObject rightBuilding = SelectBuilding(Side.Right);
            Building rightBuildingData = rightBuilding.GetComponent<Building>(); ;
            //Put buildings
            float leftBuildingPositionX = -(wallWidth / 2) - (leftBuildingData.width / 2) - distanceToRoad;
            float leftBuildingPositionY = leftBuildingData.height / 2 - (leftBuildingData.height * 0.15f);
            float leftBuildingPositionZ = currentLeftStreetLength + (leftBuildingData.length / 2);
            leftBuilding.transform.position = new Vector3(leftBuildingPositionX, leftBuildingPositionY, leftBuildingPositionZ);
            currentLeftStreetLength = currentLeftStreetLength + leftBuildingData.length + distanceBeetwenBildings;
            SetColors(leftBuilding);
            float rightBuildingPositionX = (wallWidth / 2) + (rightBuildingData.width / 2) + distanceToRoad;
            float rightBuildingPositionY = rightBuildingData.height / 2 - (rightBuildingData.height * 0.15f); ;
            float rightBuildingPositionZ = currentRightStreetLength + (rightBuildingData.length / 2);
            rightBuilding.transform.position = new Vector3(rightBuildingPositionX, rightBuildingPositionY, rightBuildingPositionZ);
            currentRightStreetLength = currentRightStreetLength + rightBuildingData.length + distanceBeetwenBildings;
            SetColors(rightBuilding);
            //Search length of current district
            if (currentLeftStreetLength > currentRightStreetLength)
                currentDistrictLength = currentDistrictLength + leftBuildingData.length + distanceBeetwenBildings;
            else
                currentDistrictLength = currentDistrictLength + rightBuildingData.length + distanceBeetwenBildings; ;
            if (currentDistrictLength >= districtLength)
            {
                AlignLegthOfDistrict();
                ChangeDistrict();
                ChangeEnvironmentColor();
                currentDistrictLength = 0;
            }
        }
        private GameObject SelectBuilding( Side side)
        {
            GameObject building = null;
            currentDistrict = District.High;
            if (currentDistrict == District.Low)
            {
                int numberOfBuildingInDistrict = 3;
                int selectedBuilding = Random.Range(1, numberOfBuildingInDistrict + 1);
                switch (selectedBuilding)
                {
                    case 1:
                        building = Instantiate(Resources.Load("bld_cube1", typeof(GameObject))) as GameObject;
                        break;
                    case 2:
                        building = Instantiate(Resources.Load("bld_cube2", typeof(GameObject))) as GameObject;
                        break;
                    case 3:
                        building = Instantiate(Resources.Load("bld_cube3", typeof(GameObject))) as GameObject;
                        break;
                }
            }
            else if(currentDistrict == District.Middle)
            {
                int numberOfBuildingInDistrict = 5;
                int selectedBuilding = Random.Range(1, numberOfBuildingInDistrict + 1);
                switch (selectedBuilding)
                {
                    case 1:
                        building = Instantiate(Resources.Load("pref_middle1", typeof(GameObject))) as GameObject;
                        
                        break;
                    case 2:
                        building = Instantiate(Resources.Load("pref_middle2", typeof(GameObject))) as GameObject;
                        break;
                    case 3:
                        building = Instantiate(Resources.Load("pref_middle3", typeof(GameObject))) as GameObject;
                        break;
                    case 4:
                        building = Instantiate(Resources.Load("pref_middle4", typeof(GameObject))) as GameObject;
                        break;
                    case 5:
                        building = Instantiate(Resources.Load("pref_middle5", typeof(GameObject))) as GameObject;
                        break;
                }
            }
            else if(currentDistrict == District.High)
            {
                int numberOfBuildingInDistrict = 5;
                int selectedBuilding = Random.Range(1, numberOfBuildingInDistrict + 1);
                switch (selectedBuilding)
                {
                    case 1:
                        building = Instantiate(Resources.Load("pref_high1", typeof(GameObject))) as GameObject;
                        break;
                    case 2:
                        building = Instantiate(Resources.Load("pref_high2", typeof(GameObject))) as GameObject;
                        break;
                    case 3:
                        building = Instantiate(Resources.Load("pref_high3", typeof(GameObject))) as GameObject;
                        break;
                    case 4:
                        building = Instantiate(Resources.Load("pref_high4", typeof(GameObject))) as GameObject;
                        break;
                    case 5:
                        building = Instantiate(Resources.Load("pref_high5", typeof(GameObject))) as GameObject;
                        break;
                }
            }
            if (side == Side.Right)
                building.transform.Rotate(new Vector3(0, -180, 0));
            return building;
        }
        private void AlignLegthOfDistrict()
        {
            Side shorterSide;
            //Search shorter side for align it with longer
            if (currentLeftStreetLength < currentRightStreetLength)
                shorterSide = Side.Left;
            else
                shorterSide = Side.Right;
            //Put nedded buildings
            bool sidesAreAlign = false;
            int numberOfIteration = 0;
            if (shorterSide == Side.Left)
            {
                while (!sidesAreAlign && !TooMuch(numberOfIteration))
                {
                    PutBuilding(Side.Left);
                    if (currentLeftStreetLength >= currentRightStreetLength)
                        sidesAreAlign = true;
                    numberOfIteration++;
                }
            }
            else if (shorterSide == Side.Right)
            {
                while (!sidesAreAlign && !TooMuch(numberOfIteration))
                {
                    PutBuilding(Side.Right);
                    if (currentRightStreetLength >= currentLeftStreetLength)
                        sidesAreAlign = true;
                    numberOfIteration ++;
                }
            }
        }
        private void ChangeDistrict()
        {
            int numberOfNewDistrict = Random.Range(1,3);
            if (currentDistrict == District.Low)
            {
                if (numberOfNewDistrict == 1)
                    currentDistrict = District.Middle;
                else if (numberOfNewDistrict == 2)
                    currentDistrict = District.High;
            }
            else if(currentDistrict == District.Middle)
            {
                if (numberOfNewDistrict == 1)
                    currentDistrict = District.Low;
                else if (numberOfNewDistrict == 2)
                    currentDistrict = District.High;
            }
            else if (currentDistrict == District.High)
            {
                if (numberOfNewDistrict == 1)
                    currentDistrict = District.Low;
                else if (numberOfNewDistrict == 2)
                    currentDistrict = District.Middle;
            }
        }
        private bool TooMuch(int numberOfIteration)
        {
            bool isTooMuch = false;
            if (numberOfIteration > 40)
                isTooMuch = true;
            return isTooMuch;
        }
        private void ChangeEnvironmentColor()
        {
            if (maxNumberOfColorSchema == 1)
                buildingColor = 0;
            else
            {
                int color = Random.Range(0,maxNumberOfColorSchema);
                if (color == buildingColor)
                    ChangeEnvironmentColor();
            }
        }
        private void SetColors(GameObject building)
        {
            building.GetComponent<MeshRenderer>().material = buildingMaterials[buildingColor];
        }
        private void PutBuilding(Side side)
        {
            if (side == Side.Left)
            {
                //Initialization
                GameObject leftBuilding = SelectBuilding(side);
                Building leftBuildingData = leftBuilding.GetComponent<Building>();
                //Put buildings
                float leftBuildingPositionX = -(wallWidth / 2) - (leftBuildingData.width / 2) - distanceToRoad;
                float leftBuildingPositionY = leftBuildingData.height / 2 - (leftBuildingData.height * 0.15f); ;
                float leftBuildingPositionZ = currentLeftStreetLength + (leftBuildingData.length / 2);
                leftBuilding.transform.position = new Vector3(leftBuildingPositionX, leftBuildingPositionY, leftBuildingPositionZ);
                currentLeftStreetLength = currentLeftStreetLength + leftBuildingData.length + distanceBeetwenBildings;
                SetColors(leftBuilding);
            }
            else if (side == Side.Right)
            {
                GameObject rightBuilding = SelectBuilding(side);
                Building rightBuildingData = rightBuilding.GetComponent<Building>(); ;
                float rightBuildingPositionX = (wallWidth / 2) + (rightBuildingData.width / 2) + distanceToRoad;
                float rightBuildingPositionY = rightBuildingData.height / 2 - (rightBuildingData.height * 0.15f); ;
                float rightBuildingPositionZ = currentRightStreetLength + (rightBuildingData.length / 2);
                rightBuilding.transform.position = new Vector3(rightBuildingPositionX, rightBuildingPositionY, rightBuildingPositionZ);
                currentRightStreetLength = currentRightStreetLength + rightBuildingData.length + distanceBeetwenBildings;
                SetColors(rightBuilding);
            }
        }
    }
    
    enum District { Low, Middle, High }
}
