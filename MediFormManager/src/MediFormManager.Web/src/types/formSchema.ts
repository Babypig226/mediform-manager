export interface ComponentOption {
    id : string;
    displayText : string;
    order : number;
}

export interface FormComponent {
   id : string;
   componentType: string;
   prompt: string | null;
   label: string | null;
   isRequired: boolean;
   isDisabled: boolean;
   isVisible: boolean;
   defaultValue: string | null;
   groupKey: string | null;
   placeholder: string | null;
   order: number;
   options: ComponentOption[];
}

export interface RuleCondition {
    id: string;
    sourceComponentId: string;
    operator: string;
    expectedOptionId: string | null;
    expectedValue: string | null;
}

export interface RuleAction {
    id: string;
    actionType: string;
    targetType: string;
    targetComponentId: string | null;
    targetGroupKey:string | null;
}

export interface ComponentRule {
    id: string;
    ruleName: string;
    logic: string;
    conditions: RuleCondition[];
    actions: RuleAction[];
}

export interface FormSchema {
    formVersionId : string;
    version : number;
    status : string;
    components : FormComponent[];
    rules : ComponentRule[];   
}