using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionsManager : MonoBehaviour
{
    public enum MissionsName
    {
        Welcome, PetTheDog, FindCandles, KillMonsters, FinishAll
    }
    private int TargetCandles = 5;
    private int TargetKill = 5;
    private int Candle;
    private int Monsters;
    private string WeaponName = "���ƻ�";
    public static MissionsManager Instance;
    public List<Mission> Missions;
    public int DialogIndex; // ��ǰ�������ͨ�Ի����������

    void Start() {
        DialogIndex = 0;
        Instance = this;
        this.Candle = GameManager.Instance.CandleNum;
        this.Monsters = GameManager.Instance.KilledMonsterNum;
        Missions = new List<Mission>() {
            // ����1 ��ӭ
            new Mission("Welcome",new DialogInfo[]
            {
                new(){Name = UiManager.NpcNames.Luna,Content = "(,,��V��)\"hello������Luna����������������ҿ������ƶ����ո����NPC���жԻ���ս������Ҫ�򵥵����ťִ����Ӧ��Ϊ"},
            },null),

            // ����2 ����
            new Mission("PetTheDog",new DialogInfo[]
            {
                new (){Name=UiManager.NpcNames.Luna,Content="�þò�����,Сè�䣨*���ئ�*��,Luna~"},
                new (){Name=UiManager.NpcNames.Nala,Content="�þò�����Nala���㻹����ô�л���������"},
                new (){Name=UiManager.NpcNames.Luna,Content="���ð�~"},
                new (){Name=UiManager.NpcNames.Nala,Content="�ҵĹ�һֱ�ڽУ����������æ�����������ܰ��Ұ���һ������"},
                new (){Name=UiManager.NpcNames.Luna,Content="����"},
                new (){Name=UiManager.NpcNames.Nala,Content="���������У�����˵�������������Ǹ��ú�����"},
                new (){Name=UiManager.NpcNames.Luna,Content="�����е���ô�ף���ʵ��������������˵�ע��"},
                new (){Name=UiManager.NpcNames.Nala,Content="���ǡ�������"},
                new (){Name=UiManager.NpcNames.Luna,Content="����èŮ�ɰ�"},
                new (){Name=UiManager.NpcNames.Nala,Content="������������ҧ���,ȥ��ȥ��~"},

            },new DialogInfo(){Name = UiManager.NpcNames.Luna,Content = "�����ڽ���"}),
            
            // ����3 ������
            new Mission("FindCandles",new DialogInfo[]
            {
                new (){Name=UiManager.NpcNames.Nala,Content="��л���ţ�Luna���㻹����ô�ɿ�!"},
                new (){Name=UiManager.NpcNames.Nala,Content="����������æ����"},
                new (){Name=UiManager.NpcNames.Nala,Content="˵�������¹��ҡ�����"},
                new (){Name=UiManager.NpcNames.Nala,Content="������˯��ͷ�ˣ����űȽϴ�æ"},
                new (){Name=UiManager.NpcNames.Nala,Content="Ȼ��װ����Ĵ��ӿ���û��ã�"},
                new (){Name=UiManager.NpcNames.Nala,Content="����͡������������������"},
                new (){Name=UiManager.NpcNames.Luna,Content="�㻹�������ӣ�����"},
                new (){Name=UiManager.NpcNames.Nala,Content="���ԣ�����ඣ�����æ�����Ұ������һ���"},
                new (){Name=UiManager.NpcNames.Nala,Content="������ܰ����һ�ȫ����"+Candle+"�������Ҿ�����һ������"},
                new (){Name=UiManager.NpcNames.Luna,Content="������"},
                new (){Name=UiManager.NpcNames.Nala,Content="�ǵģ��Ҹо����ʺ��㣬������~"},

            },new DialogInfo(){Name = UiManager.NpcNames.Nala,Content = "�㻹û�����ռ������е����򣬱�~"}),

            // ����4 �������,���Դ�����ʾ����,Ȼ���������
            new Mission("KillMonsters",new DialogInfo[]
            {
                new (){Name=UiManager.NpcNames.Nala,Content="�ɿ�������Ȼһ�������ȫ�ռ�����"},
                new (){Name=UiManager.NpcNames.Luna,Content="��֪��������"},
                new (){Name=UiManager.NpcNames.Luna,Content="�㵽���ܣ���ĺ����ռ�"},
                new (){Name=UiManager.NpcNames.Nala,Content="������������"},
                new (){Name=UiManager.NpcNames.Nala,Content="���Ǹ���Ľ���"},
                new (){Name=UiManager.NpcNames.Nala,Content= WeaponName + "����˵�е�����"},
                new (){Name=UiManager.NpcNames.Nala,Content="Ӧ��ͦ�ʺ����"},
                new (){Name=UiManager.NpcNames.Luna,Content="~~���"+WeaponName+"~~(��������ɴ���ս��)"},
                new (){Name=UiManager.NpcNames.Luna,Content="�ۣ�лл�㣡Thanks�����ء�)"},
                new (){Name=UiManager.NpcNames.Nala,Content="�ٺ٣�*^v^*�������ǵĹ�ϵ���ÿ���"},
                new (){Name=UiManager.NpcNames.Nala,Content="���ã����ɽ�������һ�ѹ����Ҳ��Ϊ���������æ������"+ Monsters +"ֻ����"},
                new (){Name=UiManager.NpcNames.Luna,Content="����"},
                new (){Name=UiManager.NpcNames.Luna,Content="����������ʵĿ�İɣ���"},
                new (){Name=UiManager.NpcNames.Nala,Content="�а�������������ĺܲ�������������"},
                new (){Name=UiManager.NpcNames.Luna,Content="�����С�����"},
                new (){Name=UiManager.NpcNames.Nala,Content="��������~"},
                new (){Name=UiManager.NpcNames.Luna,Content="�����аɣ�˭�������~"},
                new (){Name=UiManager.NpcNames.Nala,Content="����������������"},

            },new DialogInfo(){Name=UiManager.NpcNames.Nala,Content="�����㻹û����ɾ��أ������Ҳ�������~"}),

            // ����5 �����������
            new Mission("FinishAll",new DialogInfo[]
            {
                new DialogInfo(){Name = UiManager.NpcNames.Nala,Content = "�����"+UiManager.NpcNames.Luna+",��Χ�ľ��񶼻�ʮ�ָ�л��ģ��л������ҼҺ�һ����"},
                new DialogInfo(){Name = UiManager.NpcNames.Luna,Content = "�Ҿ��ÿ��У�����~"},
            },new DialogInfo(){ Name = UiManager.NpcNames.Nala, Content = "�����ټ��~" }),
             
            // ����6 ...
            //new Mission("E",new DialogInfo[]
            //{

            //},new DialogInfo(){}),
        };
    }

}
