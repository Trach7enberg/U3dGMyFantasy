using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



/// <summary>
/// �Ի���Ϣ
/// </summary>
public class NpcDialog : MonoBehaviour
{
    
    private string Candle = "5";
    private string Monsters = "5";
    private string WeaponName = "���ƻ�";
    private List<DialogInfo[]> list;
    public int CurrentContentIndex;

    private void Start() {
        
        CurrentContentIndex = 0;
        list = new List<DialogInfo[]>() {
            new DialogInfo[] {
                new DialogInfo(){Name = UIManager.NpcNames.Luna,Content = "(,,��V��)\"hello������Luna����������������ҿ������ƶ����ո����NPC���жԻ���ս������Ҫ�򵥵����ťִ����Ӧ��Ϊ"},
            },
            //1
            new DialogInfo[] {
                new DialogInfo(){Name=UIManager.NpcNames.Luna,Content="�þò�����,Сè�䣨*���ئ�*��,Luna~"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="�þò�����Nala���㻹����ô�л���������"},
                new DialogInfo(){Name=UIManager.NpcNames.Luna,Content="���ð�~"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="�ҵĹ�һֱ�ڽУ����������æ�����������ܰ��Ұ���һ������"},
                new DialogInfo(){Name=UIManager.NpcNames.Luna,Content="����"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="���������У�����˵�������������Ǹ��ú�����"},
                new DialogInfo(){Name=UIManager.NpcNames.Luna,Content="�����е���ô�ף���ʵ��������������˵�ע��"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="���ǡ�������"},
                new DialogInfo(){Name=UIManager.NpcNames.Luna,Content="����èŮ�ɰ�"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="������������ҧ���,ȥ��ȥ��~"},

            },
            //2
            new DialogInfo[] {
                new DialogInfo(){Name = UIManager.NpcNames.Luna,Content = "�����ڽ���"},
            },
            //3
            new DialogInfo[] {
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="��л���ţ�Luna���㻹����ô�ɿ�!"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="����������æ����"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="˵�������¹��ҡ�����"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="������˯��ͷ�ˣ����űȽϴ�æ"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="Ȼ��װ����Ĵ��ӿ���û��ã�"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="����͡������������������"},
                new DialogInfo(){Name=UIManager.NpcNames.Luna,Content="�㻹�������ӣ�����"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="���ԣ�����ඣ�����æ�����Ұ������һ���"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="������ܰ����һ�ȫ����"+Candle+"�������Ҿ�����һ������"},
                new DialogInfo(){Name=UIManager.NpcNames.Luna,Content="������"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="�ǵģ��Ҹо����ʺ��㣬������~"},
            },
            //4
            new DialogInfo[] {
                new DialogInfo(){Name = UIManager.NpcNames.Nala,Content = "�㻹û�����ռ������е����򣬱�~"},
            },
            //5
            new DialogInfo[] {
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="�ɿ�������Ȼһ�������ȫ�ռ�����"},
                new DialogInfo(){Name=UIManager.NpcNames.Luna,Content="��֪��������"},
                new DialogInfo(){Name=UIManager.NpcNames.Luna,Content="�㵽���ܣ���ĺ����ռ�"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="������������"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="���Ǹ���Ľ���"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content= WeaponName + "����˵�е�����"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="Ӧ��ͦ�ʺ����"},
                new DialogInfo(){Name=UIManager.NpcNames.Luna,Content="~~���"+WeaponName+"~~(��������ɴ���ս��)"},
                new DialogInfo(){Name=UIManager.NpcNames.Luna,Content="�ۣ�лл�㣡Thanks�����ء�)"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="�ٺ٣�*^v^*�������ǵĹ�ϵ���ÿ���"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="���ã����ɽ�������һ�ѹ����Ҳ��Ϊ���������æ������"+ Monsters +"ֻ����"},
                new DialogInfo(){Name=UIManager.NpcNames.Luna,Content="����"},
                new DialogInfo(){Name=UIManager.NpcNames.Luna,Content="����������ʵĿ�İɣ���"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="�а�������������ĺܲ�������������"},
                new DialogInfo(){Name=UIManager.NpcNames.Luna,Content="�����С�����"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="��������~"},
                new DialogInfo(){Name=UIManager.NpcNames.Luna,Content="�����аɣ�˭�������~"},
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="����������������"},
            },
            //6
            new DialogInfo[] {
                new DialogInfo(){Name=UIManager.NpcNames.Nala,Content="�����㻹û����ɾ��أ������Ҳ�������~"},
            },
            //7
            new DialogInfo[] {
                new DialogInfo(){Name = UIManager.NpcNames.Nala,Content = "�����"+UIManager.NpcNames.Luna+",��Χ�ľ��񶼻�ʮ�ָ�л��ģ��л������ҼҺ�һ����"},
                new DialogInfo(){Name = UIManager.NpcNames.Luna,Content = "�Ҿ��ÿ��У�����~"},
            },
            //8
            new DialogInfo[] {
                new DialogInfo() { Name = UIManager.NpcNames.Nala, Content = "�����ټ��~" }
            }
        };
    }

    /// <summary>
    /// ��ʾ�Ի�
    /// </summary>
    public void DisplayDialog() {
        Debug.Log("listCurrentLen: "+ GameManager.Instance.CurrentDialogInfoIndex+ ",listLen: "+ (list.Count - 1));
        // ��ǰһά���������������±��򷵻�
        if (!(GameManager.Instance.CurrentDialogInfoIndex < list.Count)) {
            return;
        }
        
        if (CurrentContentIndex < list[GameManager.Instance.CurrentDialogInfoIndex].Length) {
            DialogInfo info = list[GameManager.Instance.CurrentDialogInfoIndex][CurrentContentIndex++];
            UIManager.Instance.ShowNpcDialog(info.Name,info.Content);
        }
        else {
            CurrentContentIndex = 0;
            UIManager.Instance.ShowNpcDialog();
            GameManager.Instance.canControlLuna = true;
        }
    }
}
