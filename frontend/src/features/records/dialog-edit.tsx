import { Button } from "@/components/ui/button"
import {
  Dialog,
  DialogContent,
  DialogDescription,
  DialogFooter,
  DialogHeader,
  DialogTitle,
} from "@/components/ui/dialog"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { Pen } from "lucide-react"
import { useEffect, useState } from "react"

import Spinner from "@/components/ui/spinner"
import { record } from "@/lib/api"
import type { PutRecordsDto, RecordsDto } from "@/types/records"
import { toast } from "sonner"

interface DialogEditRecordsProps {
  recordId: number
  open?: boolean
  onOpenChange?: (open: boolean) => void
}

export default function DialogEditRecords({
  recordId,
  open: controlledOpen,
  onOpenChange
}: DialogEditRecordsProps) {
  const [isLoading, setIsLoading] = useState(false)

  const [recordName, setRecordName] = useState("")
  const [target, setTarget] = useState("")
  const [priority, setPriority] = useState(0)
  const [ttl, setTtl] = useState(0)
  const [type, setType] = useState("")

  const handleChangeName = (event: React.ChangeEvent<HTMLInputElement>) => setRecordName(event.target.value);
  const handleChangeTarget = (event: React.ChangeEvent<HTMLInputElement>) => setTarget(event.target.value);
  const handleChangePriority = (event: React.ChangeEvent<HTMLInputElement>) => setPriority(parseInt(event.target.value));
  const handleChangeTtl = (event: React.ChangeEvent<HTMLInputElement>) => setTtl(parseInt(event.target.value));
  const handleChangeType = (event: React.ChangeEvent<HTMLInputElement>) => setType(event.target.value);

  useEffect(() => {
    async function fetchRecordDetails(recordId: number) {
      try {
        const recordDetails: RecordsDto = await record.getById(recordId)
        setRecordName(recordDetails.recordName || "")
        setTarget(recordDetails.target || "")
        setPriority(recordDetails.priority || 0)
        setTtl(recordDetails.ttl || 0)
        setType(recordDetails.recordType || "")
      // eslint-disable-next-line @typescript-eslint/no-unused-vars
      } catch (error) {
        toast.error("Failed to fetch record details.")
      }
    }

    if (recordId && controlledOpen) {
      fetchRecordDetails(recordId)
    }
  }, [recordId, controlledOpen])

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault()
    setIsLoading(true)

    const recordData: PutRecordsDto = {
      recordName: recordName,
      target: target,
      recordType: type,
      priority: priority,
      ttl: ttl,
    }

    try {
      const result = await record.put(recordId, recordData)
      
      if (result) {
        toast.success("Record updated successfully!")
        
        if (onOpenChange) {
          onOpenChange(false)
        }
      }
      
    } catch (error) {
      toast.error(error?.message || "An error occurred while updating the record.")
    } finally {
      setIsLoading(false)
    }
  }

  return (
    <>
      {/* Custom button to open dialog */}
      <Button 
        className="w-full justify-start" 
        variant="ghost"
        onClick={() => onOpenChange && onOpenChange(true)}
      >
        <Pen className="mr-2 h-4 w-4" />
        Edit Record
      </Button>
      
      {/* Dialog */}
      <Dialog open={controlledOpen} onOpenChange={onOpenChange}>
        <DialogContent className="sm:max-w-[425px]" onInteractOutside={(e) => e.preventDefault()}>
          <form onSubmit={handleSubmit}>
            <DialogHeader>
              <DialogTitle>Edit Record</DialogTitle>
              <DialogDescription>
                Edit the record details.
              </DialogDescription>
            </DialogHeader>
            <div className="grid gap-4 py-4">
              <div className="grid gap-3">
                <Label htmlFor="recordName">Name</Label>
                <Input
                  id="recordName"
                  name="recordName"
                  defaultValue={recordName}
                  onChange={handleChangeName}
                  required
                />
              </div>
              <div className="grid gap-3">
                <Label htmlFor="target">Target</Label>
                <Input
                  id="target"
                  name="target"
                  defaultValue={target}
                  onChange={handleChangeTarget}
                  required
                />
              </div>
              <div className="grid gap-3">
                <Label htmlFor="type">Type</Label>
                <Input
                  id="type"
                  name="type"
                  defaultValue={type}
                  onChange={handleChangeType}
                  required
                />
              </div>
              <div className="grid gap-3">
                <Label htmlFor="priority">Priority</Label>
                <Input
                  id="priority"
                  name="priority"
                  type="number"
                  defaultValue={priority}
                  onChange={handleChangePriority}
                  required
                />
              </div>
              <div className="grid gap-3">
                <Label htmlFor="ttl">TTL</Label>
                <Input
                  id="ttl"
                  name="ttl"
                  type="number"
                  defaultValue={ttl}
                  onChange={handleChangeTtl}
                  required
                />
              </div>
            </div>
            <DialogFooter>
              <Button
                type="button"
                variant="outline"
                onClick={() => onOpenChange && onOpenChange(false)}
              >
                Cancel
              </Button>
              <Button
                type="submit"
                disabled={isLoading || !recordName || !target || !type}
              >
                {isLoading ? <Spinner /> : "Update Record"}
              </Button>
            </DialogFooter>
          </form>
        </DialogContent>
      </Dialog>
    </>
  )
}