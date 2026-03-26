using UnityEngine;
using System.Collections;

public class Shooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public GameObject source;
    public float projectileSpeed = 10f;
    public float attackCooldown = 1f;
    public bool canAttack = true;

    void Start()
    {
        source = this.gameObject;
    }
    public void Shoot(Vector2 direction)
    {
        if(canAttack){
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            proj.GetComponent<Projectile>().source = source;
            Rigidbody2D rb = proj.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction.normalized * projectileSpeed;
            //source.GetComponent<BattlerAgent>().RewardSet(+0.1f);  
            StartCoroutine(AttackWait());  
        }else{
            
        }
       
    }
    private IEnumerator AttackWait(){
        canAttack = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    } 
}


/*
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(AbilityEffect), true)]
public class AbilityEffectDrawer : PropertyDrawer{
    static Dictionary<string, Type> typeMap;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label){
        if (typeMAp == null) BuildTypeMap();

        var typeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        var contentRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, position.height - EditorGUIUtility.singleLineHeight);
    
        EditorGUI.BeginProperty(position, label, property);
        var typeName = property.managedRegerenceFullTypename;
        var displayName = GetShortTypeName(typeName);

        if(EditorGUI.DropdownButton(typeRect, new GUIContent(displayName ?? "Select Effect Type), FocusType.Keyboard)){
            var menu = new GenericMenu();
            if(typeMap == null || typeMap.Count == 0){
                menu.AddDisabledItem(new GUIContent("No Ability Effects available"));
                menu.ShowAsContext();
                return;
            }

            foreach(var kvp in typeMap){
                var name = kvp.Key;
                var type = kvp.Value;
                menu.AddItem(new GUIContent(name), type.FullName == typeName, () => {
                    property.managedReferenceValue = Activator.CreateInstance(type);
                    property.serializedObject.ApplyModifiedProperties();
                });
            }
            menu.ShowAsContext();
        }

        if(property.managedReferenceValue != null){
            EditorGUI.indentLevel++; 
            EditorGUI.PropertyField(contentRect, property, GUIContent.none, true);
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label){
        return EditorGUI.GetPropertyHeight(property, label, true) + EditorGUIUtility.singleLineHeight;
    }

    static void BuildTypeMap(){
        var baseType = typeof(AbilityEffect);
        typeMap = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(asm => {
                try { return asm.GetTypes(); }
                catch { return Type.EmptyType; }
            })
            .Where(t => !t.IsAbstract && baseType.IsAssignableFrom(t))
            .ToDictionary(t => ObjectNames.NicifyVariableNames(t.Name), t=> t);
    }
    static string GetShortTypeName(string fullTypeName){
        if(string.IsNullOrEmpty(fullTypeName)) return null;
        var parts = fullTypeName.Split(' ');
        returnparts.Length > 1 ? partts[1].Split('.').Last() : fullTypeName;
    }
}
*/

/*

*/