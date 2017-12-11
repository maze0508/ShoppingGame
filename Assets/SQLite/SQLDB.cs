using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class SQLDB:MonoBehaviour {

	Database db;//資料庫物件

	string databaseName = "FruitDB.db";//資料庫名稱
	string tableName = "Fruit";//資料庫內的資料表名稱
	SqliteDataReader reader;//搜尋資料表的資料

	string show = "";//要顯示在螢幕上的內容

	public void Awake()
	{
		db = new Database(databaseName);//建立資料庫，同時進行連線
		db.openDatabaseConnecting();//開啟資料庫
		if (db.isTableExists(tableName) == false)//確認是否已有指定的資料表
		{
			Debug.Log ("Table isn't exist.");
		}
	}

	public string getFruitInfo(string s_searchColumn)
	{
		s_searchColumn = "'"+s_searchColumn+"'";
		//搜尋和讀取符合的資料
		reader = db.searchAccordData(tableName, "FruitName", "=", s_searchColumn);
		string[] data = db.readStringData(reader, "FruitInfo");
		//將讀取到的第一筆資料顯示出來
		show = data[0].ToString();	
		return show;

	}
	
	public string[] getFruitByLevel(int i_level){
		reader = db.searchAccordData(tableName, "Level", "=", i_level+"");
		string[] data = db.readStringData(reader, "FruitName");
		//show = data[0].ToString();
		return data;
	}

	public void closeDBConnecting()
	{
		db.closeDatabaseConnecting();//當該程式碼所放置的物件被銷毀時關閉資料庫連線
	}

	void OnDestroy()
	{
		//釋放資料庫
		db.releaseDatabaseAllResources();
	}



}
