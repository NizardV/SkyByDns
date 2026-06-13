import { Button } from "@/components/ui/button"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
  DialogTrigger,
} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { PlusCircle, Check } from "lucide-react"
import Spinner from "@/components/ui/spinner"

interface DialogAddProps {
    handleChangeName: (event: React.ChangeEvent<HTMLInputElement>) => void;
    handleSubmit: (event: React.FormEvent<HTMLFormElement>) => void;
    domainExists: boolean;
    isLoading: boolean;
    canSubmit: boolean;
    errorState: string;
    domainName: string;
}

export default function DialogAdd({
    handleChangeName,
    handleSubmit,
    domainExists,
    isLoading,
    canSubmit,
    errorState,
    domainName
}: DialogAddProps) {
    return (
        <Dialog>
            <DialogTrigger asChild>
                <Button className="mt-4 w-full" id="feature-3">
                    <PlusCircle className="mr-2" />
                    Add DNS Record
                </Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <form onSubmit={handleSubmit}>
                    <DialogHeader>
                        <DialogTitle>Add DNS Record</DialogTitle>
                        <DialogDescription>
                            Add a new DNS record.
                        </DialogDescription>
                    </DialogHeader>
                    <div className="grid gap-4 py-4">
                        <div className="grid gap-3">
                        <Label htmlFor="name">Domain Name</Label>
                        <Input
                            id="name"
                            name="name"
                            onChange={handleChangeName}
                            value={domainName}
                        />
                        {domainName && (
                            <>
                            {domainExists === true && (
                                <p className="mt-2 flex items-center text-sm text-red-600">
                                <PlusCircle className="mr-1 h-4 w-4" />
                                Domain already exists
                                </p>
                            )}
                            {domainExists === false && !errorState && (
                                <p className="mt-2 flex items-center text-sm text-green-600">
                                <Check className="mr-1 h-4 w-4" />
                                Domain is available
                                </p>
                            )}
                            {errorState && (
                                <div className="mt-2 text-sm text-red-600">Error: {errorState}</div>
                            )}
                            </>
                        )}
                        </div>
                    </div>
                    <DialogFooter>
                        <Button className="w-full" type="submit" disabled={!canSubmit}>{isLoading ? <Spinner /> : "Save changes"}</Button>
                    </DialogFooter>
                </form>
            </DialogContent>
        </Dialog>
    )
}